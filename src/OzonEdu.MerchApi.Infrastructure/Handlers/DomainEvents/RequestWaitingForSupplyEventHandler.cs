using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.HumanResourceManagerAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Domain.Events;
using OzonEdu.MerchApi.Domain.Exceptions;

namespace OzonEdu.MerchApi.Infrastructure.Handlers.DomainEvents
{
    public class RequestWaitingForSupplyEventHandler : INotificationHandler<RequestWaitingForSupplyEvent>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHumanResourceManagerRepository _humanResourceManagerRepository;
        private readonly IMessageBus _messageBus;

        public RequestWaitingForSupplyEventHandler(IEmployeeRepository employeeRepository, IMessageBus messageBus,
            IHumanResourceManagerRepository humanResourceManagerRepository)
        {
            _employeeRepository = employeeRepository;
            _messageBus = messageBus;
            _humanResourceManagerRepository = humanResourceManagerRepository;
        }

        public async Task Handle(RequestWaitingForSupplyEvent notification, CancellationToken cancellationToken)
        {
            var manager =
                await _humanResourceManagerRepository.GetAsync(notification.Request.ManagerId.Value, cancellationToken);
            if (manager == null) throw new ManagerNotFoundException();

            var employee = await _employeeRepository.GetAsync(manager.EmployeeId.Value, cancellationToken);
            if (employee == null) throw new EmployeeNotFoundException();

            await _messageBus.NotifyAsync(EmailMessage.Create(employee.Email,
                $"Отсутсвует мерч {notification.Request.RequestedMerchType} на складе"));
        }
    }
}