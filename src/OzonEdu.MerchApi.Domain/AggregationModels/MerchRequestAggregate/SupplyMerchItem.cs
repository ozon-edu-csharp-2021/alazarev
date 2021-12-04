using System;
using System.Collections.Generic;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate
{
    public sealed class SupplyMerchItem : ValueObject
    {
        public Sku Sku { get; private set; }

        public SupplyMerchItem(Sku sku)
        {
            Sku = sku ?? throw new ArgumentNullException(nameof(sku));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Sku;
        }
    }
}