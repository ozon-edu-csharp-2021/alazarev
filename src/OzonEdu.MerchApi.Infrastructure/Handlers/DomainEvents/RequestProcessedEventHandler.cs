using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchApi.Domain.Events;

namespace OzonEdu.MerchApi.Infrastructure.Handlers.DomainEvents
{
    public class RequestProcessedEventHandler : INotificationHandler<RequestProcessedEvent>
    {
        public Task Handle(RequestProcessedEvent requestProcessedEvent, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}