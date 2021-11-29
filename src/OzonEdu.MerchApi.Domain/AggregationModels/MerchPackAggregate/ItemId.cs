using System;
using System.Collections.Generic;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate
{
    public sealed class ItemId : ValueObject
    {
        public long Value { get; }

        public ItemId(long id)
        {
            if (id <= 0) throw new Exception("Incorrect id");
            Value = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}