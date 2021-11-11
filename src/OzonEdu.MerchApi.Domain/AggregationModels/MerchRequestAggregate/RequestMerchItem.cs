using System;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate
{
    public class RequestMerchItem : Entity
    {
        public Sku Sku { get; private set; }
        public RequestMerchItemStatus Status { get; private set; }

        public RequestMerchItem(Sku sku)
        {
            Sku = sku;
            Status = RequestMerchItemStatus.NotVerified;
        }

        public bool ChangeStatus(RequestMerchItemStatus status)
        {
            if (Equals(status, Status)) return false;
            Status = status;
            return true;
        }
    }
}