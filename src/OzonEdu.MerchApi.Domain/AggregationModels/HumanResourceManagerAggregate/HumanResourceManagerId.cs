using System.Collections.Generic;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.HumanResourceManagerAggregate
{
    public sealed class HumanResourceManagerId : ValueObject
    {
        public long Value { get; }

        public HumanResourceManagerId(long id)
        {
            Value = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}