using System;
using System.Collections.Generic;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate
{
    public sealed class Quantity : ValueObject
    {
        public int Value { get; }

        public Quantity(int quantity)
        {
            if (quantity <= 0) throw new Exception("Incorrect quantity");
            Value = quantity;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}