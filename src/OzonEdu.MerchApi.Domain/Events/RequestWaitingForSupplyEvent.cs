using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;

namespace OzonEdu.MerchApi.Domain.Events
{
    public class RequestWaitingForSupplyEvent : INotification
    {
        public RequestWaitingForSupplyEvent(MerchRequest request)
        {
            Request = request;
        }

        public MerchRequest Request { get; }
    }
}