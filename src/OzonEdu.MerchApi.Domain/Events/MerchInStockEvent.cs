using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;

namespace OzonEdu.MerchApi.Domain.Events
{
    public class MerchInStockEvent : INotification
    {
        public MerchInStockEvent(MerchRequest request)
        {
            Request = request;
        }


        public MerchRequest Request { get; }
    }
}