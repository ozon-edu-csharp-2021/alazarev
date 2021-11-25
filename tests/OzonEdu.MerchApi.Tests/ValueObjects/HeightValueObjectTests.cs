using System;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using Xunit;

namespace OzonEdu.MerchApi.Tests.ValueObjects
{
    public class HeightValueObjectTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-200)]
        [InlineData(250.1)]
        public void CreateHeight_WhenIncorrectMetricHeight(double metricHeight)
        {
            var exception = Assert.Throws<Exception>(() => Height.FromMetrics(metricHeight));
            Assert.Equal("Incorrect height", exception.Message);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 100)]
        public void CreateHeight_WhenIncorrectImperialHeight(int feet, int inches)
        {
            var exception = Assert.Throws<Exception>(() => Height.FromImperial(feet, inches));
            Assert.Equal("Incorrect height", exception.Message);
        }

        [Fact]
        public void CreateHeight_WhenCorrectMetricHeight()
        {
            Height.FromMetrics(250);
            Assert.True(true);
        }

        [Fact]
        public void CreateHeight_WhenCorrectImperialHeight()
        {
            Height.FromImperial(2, 6);
            Assert.True(true);
        }
    }
}