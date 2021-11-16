using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
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
        private readonly IMerchRequestService _merchRequestService;
        private readonly IValidator<GetEmployeeMerchRequestsQuery> _validator;

        public GetEmployeeMerchRequestsQueryHandler(IMerchRequestService merchRequestService,
            IValidator<GetEmployeeMerchRequestsQuery> validator)
        {
            _merchRequestService = merchRequestService;
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
                await _merchRequestService.GetMerchInfoAsync(Email.Create(request.EmployeeEmail), cancellationToken);

            return GetEmployeeMerchRequestsResult.Success(requests, "Записи успешно получены");
        }
    }
}