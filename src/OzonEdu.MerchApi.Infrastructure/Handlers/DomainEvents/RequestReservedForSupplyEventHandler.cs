using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchApi.Domain.Events;

namespace OzonEdu.MerchApi.Infrastructure.Handlers.DomainEvents
{
    public class RequestReservedForSupplyEventHandler:INotificationHandler<RequestReservedEvent>
    {
        public Task Handle(RequestReservedEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}