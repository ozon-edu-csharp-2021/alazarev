using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.Exceptions;

namespace OzonEdu.MerchApi.Domain.Events
{
    public class RequestReservedEvent : INotification
    {
        public RequestReservedEvent(MerchRequest request)
        {
            Request = request;
        }

        public MerchRequest Request { get; }
    }
}