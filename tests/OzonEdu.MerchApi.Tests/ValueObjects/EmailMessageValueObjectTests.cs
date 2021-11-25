using System;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using Xunit;

namespace OzonEdu.MerchApi.Tests.ValueObjects
{
    public class EmailMessageValueObjectTests
    {
        public static object[][] CreateEmailMessage_SomeArgumentIsNull_Data =
        {
            new object[] { null, "Hi!" },
            new object[] { Email.Create("very@common_ema.il"), null }
        };

        [Theory]
        [MemberData(nameof(CreateEmailMessage_SomeArgumentIsNull_Data))]
        public void CreateEmailMessage_SomeArgumentIsNull(Email emailAddress, string body)
        {
            Assert.Throws<ArgumentNullException>(() => EmailMessage.Create(emailAddress, body));
        }
    }
}