using System;
using System.Collections.Generic;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate
{
    public class SupplyMerchItem : ValueObject
    {
        public Sku Sku { get; private set; }
        public Quantity Quantity { get; private set; }

        public SupplyMerchItem(Sku sku, Quantity quantity)
        {
            Sku = sku ?? throw new ArgumentNullException(nameof(sku));
            Quantity = quantity ?? throw new ArgumentNullException(nameof(quantity));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Sku;
        }
    }
}