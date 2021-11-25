using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Contracts.DomainServices.MerchRequestService;
using OzonEdu.MerchApi.Infrastructure.Commands.CreateMerchRequest;
using OzonEdu.MerchApi.Infrastructure.Extensions;
using OzonEdu.MerchApi.Infrastructure.Queries.GetEmployeeMerchRequests;

namespace OzonEdu.MerchApi.Infrastructure.Handlers.Queries
{
    public class
        GetEmployeeMerchRequestsQueryHandler : IRequestHandler<GetEmployeeMerchRequestsQuery,
            GetEmployeeMerchRequestsResult>
    {
        private readonly IMerchRequestRepository _merchRequestRepository;
        private readonly IValidator<GetEmployeeMerchRequestsQuery> _validator;

        public GetEmployeeMerchRequestsQueryHandler(IMerchRequestRepository merchRequestRepository,
            IValidator<GetEmployeeMerchRequestsQuery> validator)
        {
            _merchRequestRepository = merchRequestRepository;
            _validator = validator;
        }

        public async Task<GetEmployeeMerchRequestsResult> Handle(GetEmployeeMerchRequestsQuery request,
            CancellationToken cancellationToken)
        {
            var validationResult =
                await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return GetEmployeeMerchRequestsResult.Fail(
                    validationResult.GetAggregateError("Произошла ошибка валидации"));

            var requests =
                await _merchRequestRepository.GetAllEmployeeRequestsAsync(EmployeeEmail.Create(request.EmployeeEmail),
                    cancellationToken);

            return GetEmployeeMerchRequestsResult.Success(requests, "Записи успешно получены");
        }
    }
}