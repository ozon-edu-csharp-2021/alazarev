using System;
using System.Collections.Generic;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate
{
    public sealed class EmployeeId : ValueObject
    {
        public int Value { get; }

        private EmployeeId(int id)
        {
            if (id < 1) throw new Exception("Wrong employee id");
            Value = id;
        }

        public static EmployeeId Create(int id) => new(id);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator EmployeeId(int id)
            => new(id);
    }
}