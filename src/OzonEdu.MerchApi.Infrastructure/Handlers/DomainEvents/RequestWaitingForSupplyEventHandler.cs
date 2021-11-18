using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Domain.Events;
using OzonEdu.MerchApi.Domain.Exceptions;

namespace OzonEdu.MerchApi.Infrastructure.Handlers.DomainEvents
{
    public class RequestWaitingForSupplyEventHandler : INotificationHandler<RequestWaitingForSupplyEvent>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMessageBus _messageBus;

        public RequestWaitingForSupplyEventHandler(IEmployeeRepository employeeRepository, IMessageBus messageBus)
        {
            _employeeRepository = employeeRepository;
            _messageBus = messageBus;
        }

        public async Task Handle(RequestWaitingForSupplyEvent notification, CancellationToken cancellationToken)
        {
            if (notification.Request.ManagerEmail == null) throw new ManagerEmailIsNullException();

            await _messageBus.NotifyAsync(EmailMessage.Create(notification.Request.ManagerEmail,
                $"Отсутсвует мерч {notification.Request.RequestedMerchType} на складе"));
        }
    }
}