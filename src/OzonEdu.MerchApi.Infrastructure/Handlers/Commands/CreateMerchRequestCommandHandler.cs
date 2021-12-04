using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Enums;
using FluentValidation;
using MediatR;
using OpenTracing;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Domain.Contracts.DomainServices.MerchRequestService;
using OzonEdu.MerchApi.Domain.Contracts.EmployeesService;
using OzonEdu.MerchApi.Domain.Contracts.StockApiService;
using OzonEdu.MerchApi.Domain.Exceptions;
using OzonEdu.MerchApi.Infrastructure.Commands.CreateMerchRequest;
using OzonEdu.MerchApi.Infrastructure.Extensions;
using OzonEdu.MerchApi.Infrastructure.StockApi;

namespace OzonEdu.MerchApi.Infrastructure.Handlers.Commands
{
    public class CreateMerchRequestCommandHandler : IRequestHandler<CreateMerchRequestCommand, CreateMerchRequestResult>
    {
        private readonly IValidator<CreateMerchRequestCommand> _validator;
        private readonly IMerchRequestRepository _merchRequestRepository;
        private readonly IMerchPackRepository _merchPackRepository;
        private readonly IStockApiService _stockApiService;
        private readonly IEmployeeApiService _employeeApiService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITracer _tracer;

        private static readonly Func<MerchRequest, bool> AlreadyHasVeteranOrConferencePacks = request =>
        {
            var merchTypeCondition =
                request.RequestedMerchType is MerchType.ConferenceListenerPack or MerchType.ConferenceSpeakerPack or
                    MerchType.VeteranPack;

            var whenReservedCondition = request.Status.Equals(MerchRequestStatus.Reserved)
                                        && request.ReservedAt.HasValue
                                        && request.ReservedAt.Value.AddYears(1) > DateTimeOffset.UtcNow;

            var inProgressCondition = request.Status.Equals(MerchRequestStatus.InProcess)
                                      || request.Status.Equals(MerchRequestStatus.WaitingForSupply);

            return merchTypeCondition
                   && ((whenReservedCondition)
                       || inProgressCondition);
        };


        private static readonly Func<MerchRequest, bool> AlreadyHasNonVeteranOrConferencePacks = request =>
        {
            var merchTypeCondition =
                request.RequestedMerchType != MerchType.ConferenceListenerPack &&
                request.RequestedMerchType != MerchType.ConferenceSpeakerPack &&
                request.RequestedMerchType != MerchType.VeteranPack;

            var reservedOrInProgressCondition = request.Status.Equals(MerchRequestStatus.Reserved)
                                                || request.Status.Equals(MerchRequestStatus.InProcess)
                                                || request.Status.Equals(MerchRequestStatus.WaitingForSupply);

            return (merchTypeCondition)
                   && (reservedOrInProgressCondition);
        };

        public CreateMerchRequestCommandHandler(
            IValidator<CreateMerchRequestCommand> validator, IMerchRequestRepository merchRequestRepository,
            IMerchPackRepository merchPackRepository, IStockApiService stockApiService,
            IEmployeeApiService employeeApiService, IUnitOfWork unitOfWork, ITracer tracer)
        {
            _validator = validator;
            _merchRequestRepository = merchRequestRepository;
            _merchPackRepository = merchPackRepository;
            _stockApiService = stockApiService;
            _employeeApiService = employeeApiService;
            _unitOfWork = unitOfWork;
            _tracer = tracer;
        }

        public async Task<CreateMerchRequestResult> Handle(CreateMerchRequestCommand command,
            CancellationToken cancellationToken)
        {
            using var span = _tracer.BuildSpan($"{nameof(CreateMerchRequestCommandHandler)}.Handle").StartActive();
            var validationResult =
                await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                return CreateMerchRequestResult.Fail(validationResult.GetAggregateError("Произошла ошибка валидации"));
            //-------------------------------

            var employee = command.Employee ??
                           await _employeeApiService.GetByIdAsync(command.EmployeeId!.Value, cancellationToken);

            if (employee == null)
                throw new Exception("Employee not found");
            var employeeId = EmployeeId.Create(employee.Id);
            var managerEmail = ManagerEmail.Create(command.ManagerEmail);

            //данный мерч доступен пользователю?
            var isMerchAvailable =
                await CheckIfMerchAvailableAsync(employeeId, command.MerchType, cancellationToken);
            if (!isMerchAvailable)
                return CreateMerchRequestResult.Fail("Невозможно создать еще одну заявку с таким типом");

            //берем мерчпак
            var merchPack = await _merchPackRepository.GetByMerchTypeAsync(command.MerchType, cancellationToken);
            if (merchPack == null)
                return CreateMerchRequestResult.Fail("Невозможно создать еще одну заявку с таким типом");

            var merchPackSkusInStock =
                await _stockApiService.GetSkusByMerchPackAndSizeAsync(merchPack, employee.ClothingSize,
                    cancellationToken);

            await _unitOfWork.StartTransaction(cancellationToken);

            //создаям заявку
            var request =
                MerchRequest.Create(employeeId, managerEmail, command.MerchRequestMode, DateTimeOffset.UtcNow);

            //устанавливаем заявке мерчпак и стартуем
            request.StartWork(merchPack, merchPackSkusInStock);

            //если стоки в наличие (заявка не перешла в ожидание)
            if (request.Status.Equals(MerchRequestStatus.InProcess))
            {
                //резервируем
                var reserveResult =
                    await _stockApiService.ReserveStockItemsAsync(request,
                        cancellationToken);

                //результат резервирования применяем к заявке
                request.Complete(reserveResult, reserveResult ? DateTimeOffset.UtcNow : null);
            }

            request = await _merchRequestRepository.CreateAsync(request, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return
                CreateMerchRequestResult.Success(request.Status, request.Id, "Заявка успешно создалась");
        }

        private async Task<bool> CheckIfMerchAvailableAsync(EmployeeId employeeId, MerchType merchType,
            CancellationToken token = default)
        {
            var employeeRequests = await _merchRequestRepository.GetAllEmployeeRequestsAsync(employeeId, token);

            if (merchType is MerchType.ConferenceListenerPack or MerchType.ConferenceSpeakerPack or MerchType
                .VeteranPack)
            {
                return !employeeRequests.Any(AlreadyHasVeteranOrConferencePacks);
            }

            return !employeeRequests.Any(AlreadyHasNonVeteranOrConferencePacks);
        }
    }
}