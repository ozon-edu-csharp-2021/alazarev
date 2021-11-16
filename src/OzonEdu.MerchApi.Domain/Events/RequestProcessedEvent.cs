using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.Exceptions;

namespace OzonEdu.MerchApi.Domain.Events
{
    public class RequestProcessedEvent : INotification
    {
        public RequestProcessedEvent(int requestId)
        {
            RequestId = requestId;
        }

        public int RequestId { get; }
    }
}