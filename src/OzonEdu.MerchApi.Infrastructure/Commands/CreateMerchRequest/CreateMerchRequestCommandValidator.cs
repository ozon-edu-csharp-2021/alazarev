using FluentValidation;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Infrastructure.Extensions;

namespace OzonEdu.MerchApi.Infrastructure.Commands.CreateMerchRequest
{
    public class CreateMerchRequestCommandValidator : AbstractValidator<CreateMerchRequestCommand>
    {
        public CreateMerchRequestCommandValidator()
        {
            RuleFor(x => x.EmployeeEmail).MustBeValidObject(Email.Create);
        }
    }
}