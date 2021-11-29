using System.Collections;
using System.Collections.Generic;
using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;

namespace OzonEdu.MerchApi.Domain.Events
{
    public class NewSupplyArrivedEvent : INotification
    {
        public IEnumerable<SupplyMerchItem> Items { get; set; }

        public NewSupplyArrivedEvent(IEnumerable<SupplyMerchItem> items)
        {
            Items = items;
        }
    }
}