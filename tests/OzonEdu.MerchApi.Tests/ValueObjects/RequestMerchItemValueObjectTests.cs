using System;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Exceptions;
using Xunit;

namespace OzonEdu.MerchApi.Tests.ValueObjects
{
    public class RequestMerchItemValueObjectTests
    {
        [Fact]
        public void Create_WithNullSku()
        {
            Assert.Throws<ArgumentNullException>(() => new RequestMerchItem(null));
        }
    }
}