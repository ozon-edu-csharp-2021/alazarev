using System;
using System.Collections.Generic;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate
{
    public sealed class MerchRequestId : ValueObject
    {
        public int Value { get; }

        public MerchRequestId(int id)
        {
            if (id < 1) throw new ArgumentException();
            Value = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator MerchRequestId(int id)
            => new(id);
    }
}