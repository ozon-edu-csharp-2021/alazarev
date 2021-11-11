using System.Collections.Generic;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate
{
    public sealed class EmployeeId : ValueObject
    {
        public int Value { get; }

        public EmployeeId(int id)
        {
            Value = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator EmployeeId(int id)
            => new(id);
    }
}