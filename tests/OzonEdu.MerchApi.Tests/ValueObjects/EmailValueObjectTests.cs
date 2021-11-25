using System;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Exceptions;
using Xunit;

namespace OzonEdu.MerchApi.Tests.ValueObjects
{
    public class EmailValueObjectTests
    {
        [Fact]
        public void CreateEmail_WhenEmailIsInvalid()
        {
            var invalidEmail = "invalidEmail";
            Assert.Throws<InvalidEmailException>(() => Email.Create(invalidEmail));
        }

        [Fact]
        public void CreateEmail_WhenEmailIsValid()
        {
            var validEmail = "email-is@val.id";

            var email =  Email.Create(validEmail);
            Assert.Equal(email.Value, validEmail);
        }

        [Fact]
        public void CreateEmail_WhenEmailIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Email.Create(null));
        }
    }
}