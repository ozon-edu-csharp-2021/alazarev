using System;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Exceptions;
using Xunit;

namespace OzonEdu.MerchApi.Tests.ValueObjects
{
    public class MerchRequestIdValueObjectTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Create_InvalidId(int id)
        {
            Assert.Throws<ArgumentException>(() => new MerchRequestId(id));
        }
    }
}