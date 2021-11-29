using System.Collections.Generic;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate
{
    public class MerchPackItem : ValueObject
    {
        public Name Name { get; private set; }
        public ItemId ItemId { get; private set; }
        public Quantity Quantity { get; private set; }

        public MerchPackItem(ItemId id, Name name, Quantity quantity)
        {
            ItemId = id;
            Name = name;
            Quantity = quantity;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ItemId;
        }
    }
}