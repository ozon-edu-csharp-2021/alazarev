using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OzonEdu.MerchApi.Domain.Exceptions;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate
{
    public class Email : ValueObject
    {
        public string Value { get; }

        public Email(string email)
        {
            if (!Regex.IsMatch(email ?? throw new ArgumentNullException(nameof(email)),
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase))
                throw new InvalidEmailException("Неправильный формат адреса электронной почты");

            Value = email;
        }

        public static Email Create(string email) => new(email);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}