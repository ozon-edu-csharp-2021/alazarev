using System.Threading;
using FluentValidation;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Infrastructure.Extensions;

namespace OzonEdu.MerchApi.Infrastructure.Commands.CreateMerchRequest
{
    public class CreateMerchRequestCommandValidator : AbstractValidator<CreateMerchRequestCommand>
    {
        public CreateMerchRequestCommandValidator()
        {
            When(x => x.Employee == null && x.EmployeeId.HasValue,
                () => { RuleFor(x => x.EmployeeId.Value).MustBeValidObject(EmployeeId.Create); });

            RuleFor(x => x.ManagerEmail).MustBeValidObject(Email.Create);
        }
    }
}