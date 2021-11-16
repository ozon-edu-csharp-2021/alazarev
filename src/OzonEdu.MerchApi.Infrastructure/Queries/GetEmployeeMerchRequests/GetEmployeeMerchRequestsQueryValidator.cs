using FluentValidation;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Infrastructure.Extensions;

namespace OzonEdu.MerchApi.Infrastructure.Queries.GetEmployeeMerchRequests
{
    public class GetEmployeeMerchRequestsQueryValidator : AbstractValidator<GetEmployeeMerchRequestsQuery>
    {
        public GetEmployeeMerchRequestsQueryValidator()
        {
            RuleFor(x => x.EmployeeEmail).MustBeValidObject(Email.Create);
        }
    }
}