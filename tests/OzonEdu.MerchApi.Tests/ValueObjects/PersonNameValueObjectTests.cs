using System;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.Exceptions;
using Xunit;

namespace OzonEdu.MerchApi.Tests.ValueObjects
{
    public class PersonNameValueObjectTests
    {
        [Theory]
        [InlineData("Ал3кс3й","Лазарев")]
        [InlineData("Алексей","Ла3арев")]
        public void CreatePersonalName_WhenFirstNameOrLastNameHasDigits(string firstName, string lastName)
        {
            Assert.Throws<InvalidPersonalNameException>(() => PersonName.Create(firstName,lastName));
        }
        
        [Theory]
        [InlineData(null,null)]
        [InlineData(null,"Лазарев")]
        [InlineData("Алексей",null)]
        public void CreatePersonalName_WhenFirstNameOrLastNameIsNull(string firstName, string lastName)
        {
            Assert.Throws<ArgumentNullException>(() => PersonName.Create(firstName, lastName));
        }
    }
}