using MediatR;

namespace OzonEdu.MerchApi.Domain.Events
{
    public class RequestOnErrorEvent : INotification
    {
        public RequestOnErrorEvent(int requestId)
        {
            RequestId = requestId;
        }

        public int RequestId { get; }
    }
}