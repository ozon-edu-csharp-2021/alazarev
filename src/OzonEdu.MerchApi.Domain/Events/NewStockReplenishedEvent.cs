using System.Collections;
using System.Collections.Generic;
using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;

namespace OzonEdu.MerchApi.Domain.Events
{
    public class NewStockReplenishedEvent : INotification
    {
        public IEnumerable<Sku> Skus { get; set; }

        public NewStockReplenishedEvent(IEnumerable<Sku> skus)
        {
            Skus = skus;
        }
    }
}