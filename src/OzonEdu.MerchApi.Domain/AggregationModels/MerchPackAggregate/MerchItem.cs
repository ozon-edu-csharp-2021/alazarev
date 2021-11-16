using System.Collections.Generic;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate
{
    public class MerchItem : Entity
    {
        public Name Name { get; private set; }
        public Sku Sku { get; private set; }
        public MerchCategory Category { get; private set; }

        public MerchItem(Sku sku, Name name, MerchCategory category)
        {
            Sku = sku;
            Name = name;
            Category = category;
        }
    }
}