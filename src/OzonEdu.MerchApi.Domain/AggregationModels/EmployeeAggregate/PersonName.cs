using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using OzonEdu.MerchApi.Domain.Exceptions;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate
{
    public class PersonName : ValueObject
    {
        public string FirstName { get; }
        public string LastName { get; }

        public PersonName(string firstName, string lastName)
        {
            if ((firstName ?? throw new ArgumentNullException(nameof(firstName))).Any(char.IsDigit))
                throw new InvalidPersonalNameException("Имя не должно содержать цифры");
            if ((lastName ?? throw new ArgumentNullException(nameof(lastName))).Any(char.IsDigit))
                throw new InvalidPersonalNameException("Фамилия не должна содержать цифры");
            FirstName = firstName;
            LastName = lastName;
        }

        public static PersonName Create(string firstName, string lastName) => new(firstName, lastName);

        public static PersonName ParseFromFullName(string fullName)
        {
            var fullNameParts = Regex.Split(fullName ?? throw new ArgumentNullException(nameof(fullName)), @"\s");
            if (fullNameParts.Length != 2)
            {
                throw new InvalidPersonalNameException("Неправильный формат полного имени");
            }

            return new PersonName(fullNameParts[0], fullNameParts[1]);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
        }
    }
}