using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Domain.Contracts.DomainServices.MerchRequestService;
using OzonEdu.MerchApi.Infrastructure.Commands.CreateMerchRequest;
using OzonEdu.MerchApi.Infrastructure.Extensions;

namespace OzonEdu.MerchApi.Infrastructure.Handlers.Commands
{
    public class CreateMerchRequestCommandHandler : IRequestHandler<CreateMerchRequestCommand, CreateMerchRequestResult>
    {
        private readonly IMerchRequestService _merchRequestService;
        private readonly IValidator<CreateMerchRequestCommand> _validator;
        private readonly IMerchRequestRepository _merchRequestRepository;

        public CreateMerchRequestCommandHandler(
            IValidator<CreateMerchRequestCommand> validator, IMerchRequestRepository merchRequestRepository,
            IMerchRequestService merchRequestService)
        {
            _validator = validator;
            _merchRequestRepository = merchRequestRepository;
            _merchRequestService = merchRequestService;
        }

        public async Task<CreateMerchRequestResult> Handle(CreateMerchRequestCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult =
                await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                return CreateMerchRequestResult.Fail(validationResult.GetAggregateError("Произошла ошибка валидации"));

            var request = await _merchRequestService.CreateMerchRequestAsync(Email.Create(command.EmployeeEmail),
                command.MerchType,
                command.MerchRequestMode, cancellationToken);

            if (request == null)
                return CreateMerchRequestResult.Fail("Невозможно создать еще одну заявку с таким типом");

            request = await _merchRequestRepository.CreateAsync(request, cancellationToken);

            await _merchRequestRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return
                CreateMerchRequestResult.Success(request.Status, request.Id, "Заявка успешно создалась");
        }
    }
}