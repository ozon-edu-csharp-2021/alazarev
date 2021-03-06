using System;
using System.Linq;
using System.Text;
using FluentValidation;
using FluentValidation.Results;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Infrastructure.Extensions
{
    public static class FluentValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> MustBeValidObject<T, TValueObject>(
            this IRuleBuilder<T, string> ruleBuilder,
            Func<string, TValueObject> factoryMethod) where TValueObject : ValueObject
            => (IRuleBuilderOptions<T, string>) ruleBuilder.Custom((value, context) =>
            {
                try
                {
                    factoryMethod(value);
                }
                catch (Exception e)
                {
                    context.AddFailure(e.Message);
                }
            });

        public static string GetAggregateError(this ValidationResult validationResult, string startErrorMessage)
        {
            if (validationResult.Errors == null || !validationResult.Errors.Any()) return startErrorMessage;
            var sb = new StringBuilder();
            sb.AppendLine(startErrorMessage);
            return validationResult.Errors.Aggregate(sb, (builder, failure) =>
            {
                builder.AppendLine(failure.ErrorMessage);
                return builder;
            }).ToString();
        }
    }
}