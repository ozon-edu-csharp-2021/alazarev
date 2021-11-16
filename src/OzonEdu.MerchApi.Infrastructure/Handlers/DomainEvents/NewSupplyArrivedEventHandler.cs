using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchApi.Domain.Events;

namespace OzonEdu.MerchApi.Infrastructure.Handlers.DomainEvents
{
    public class NewSupplyArrivedEventHandler:INotificationHandler<NewSupplyArrivedEvent>
    {
        public Task Handle(NewSupplyArrivedEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}