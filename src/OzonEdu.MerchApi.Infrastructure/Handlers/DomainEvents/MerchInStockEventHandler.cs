using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Domain.Events;
using OzonEdu.MerchApi.Domain.Exceptions;

namespace OzonEdu.MerchApi.Infrastructure.Handlers.DomainEvents
{
    public class MerchInStockEventHandler : INotificationHandler<MerchInStockEvent>
    {
        private readonly IMessageBus _messageBus;

        public MerchInStockEventHandler(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public async Task Handle(MerchInStockEvent notification, CancellationToken cancellationToken)
        {
            //TODO GET EMPLOYEE

            // await _messageBus.NotifyAsync(EmailMessage.Create(notification.Request.EmployeeId,
            //     "Мерч появился на складе"));
        }
    }
}