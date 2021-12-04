using System;
using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;

namespace OzonEdu.MerchApi.Domain.Events
{
    public class MerchRequestCanceledEvent : INotification
    {
        public MerchRequestCanceledEvent(MerchRequest merchRequest)
        {
            throw new NotImplementedException();
        }
    }
}