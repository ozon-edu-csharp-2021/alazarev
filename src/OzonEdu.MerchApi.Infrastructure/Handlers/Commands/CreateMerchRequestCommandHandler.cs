using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Enums;
using FluentValidation;
using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Domain.Contracts.DomainServices.MerchRequestService;
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

        public CreateMerchRequestCommandHandler(
            IValidator<CreateMerchRequestCommand> validator, IMerchRequestRepository merchRequestRepository,
            IMerchPackRepository merchPackRepository, IStockApiService stockApiService)
        {
            _validator = validator;
            _merchRequestRepository = merchRequestRepository;
            _merchPackRepository = merchPackRepository;
            _stockApiService = stockApiService;
        }

        public CreateMerchRequestCommandHandler(IMerchPackRepository merchPackRepository,
            IStockApiService stockApiService)
        {
            _merchPackRepository = merchPackRepository;
            _stockApiService = stockApiService;
        }

        public async Task<CreateMerchRequestResult> Handle(CreateMerchRequestCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult =
                await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                return CreateMerchRequestResult.Fail(validationResult.GetAggregateError("Произошла ошибка валидации"));
            //-------------------------------
            var employeeEmail = EmployeeEmail.Create(command.EmployeeEmail);
            var managerEmail = ManagerEmail.Create(command.ManagerEmail);

            //данный мерч доступен пользователю?
            var isMerchAvailable =
                await CheckIfMerchAvailableAsync(employeeEmail, command.MerchType, cancellationToken);
            if (!isMerchAvailable)
                return CreateMerchRequestResult.Fail("Невозможно создать еще одну заявку с таким типом");

            //берем мерчпак
            var merchPack = await _merchPackRepository.GetByMerchTypeAsync(command.MerchType, cancellationToken);
            if (merchPack == null)
                return CreateMerchRequestResult.Fail("Невозможно создать еще одну заявку с таким типом");

            //получаем ску со склада по мерч паку
            //TODO размер пока произвольно ставим. в будущем нужно размер для сотрудника запрашивать
            var merchPackSkusInStock =
                await _stockApiService.GetSkusByMerchPackAndSizeAsync(merchPack, command.ClothingSize,
                    cancellationToken);

            //создаям заявку
            var request = MerchRequest.Create(employeeEmail, managerEmail, command.ClothingSize,
                command.MerchRequestMode,
                DateTimeOffset.UtcNow
            );

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

            return
                CreateMerchRequestResult.Success(request.Status, request.Id, "Заявка успешно создалась");
        }

        private async Task<bool> CheckIfMerchAvailableAsync(EmployeeEmail employeeEmail, MerchType merchType,
            CancellationToken token = default)
        {
            var employeeRequests = await _merchRequestRepository.GetAllEmployeeRequestsAsync(employeeEmail, token);

            if (merchType is MerchType.ConferenceListenerPack or MerchType.ConferenceSpeakerPack or MerchType
                .VeteranPack)
            {
                return !employeeRequests.Any(r =>
                    r.RequestedMerchType == merchType
                    && ((r.Status.Equals(MerchRequestStatus.Reserved)
                         && r.ReservedAt.HasValue
                         && r.ReservedAt.Value.AddYears(1) > DateTimeOffset.UtcNow)
                        || r.Status.Equals(MerchRequestStatus.InProcess)
                        || r.Status.Equals(MerchRequestStatus.WaitingForSupply))
                );
            }

            return !employeeRequests.Any(r =>
                r.RequestedMerchType == merchType
                && (r.Status.Equals(MerchRequestStatus.Reserved)
                    || r.Status.Equals(MerchRequestStatus.InProcess)
                    || r.Status.Equals(MerchRequestStatus.WaitingForSupply))
            );
        }
    }
}