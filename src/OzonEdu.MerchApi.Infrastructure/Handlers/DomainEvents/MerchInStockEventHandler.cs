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
    public class MerchInStockEventHandler : INotificationHandler<MerchInStockEvent>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMessageBus _messageBus;

        public MerchInStockEventHandler(IEmployeeRepository employeeRepository, IMessageBus messageBus)
        {
            _employeeRepository = employeeRepository;
            _messageBus = messageBus;
        }

        public async Task Handle(MerchInStockEvent notification, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetAsync(notification.EmployeeId.Value, cancellationToken);
            if (employee == null) throw new EmployeeNotFoundException();

            await _messageBus.NotifyAsync(new EmailMessage(employee.Email, "Мерч появился на складе"));
        }
    }
}