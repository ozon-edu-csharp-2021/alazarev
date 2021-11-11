using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Enums;
using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.Events;

namespace OzonEdu.MerchApi.Infrastructure.Handlers.DomainEvents
{
    public class EmployeeNotificationDomainEventHandler : INotificationHandler<EmployeeNotificationDomainEvent>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeNotificationDomainEventHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task Handle(EmployeeNotificationDomainEvent notification, CancellationToken cancellationToken)
        {
            if (notification.EventType == EmployeeEventType.Hiring)
            {
                // TODO в будущем нужна информация о размере сотрудника и росте, чтобы правильно формировать мерч пак для сотрудника.
                // Возможно за этой информацией Merch Api будем сам лазить в сервис сотрудников
                await _employeeRepository.CreateAsync(new Employee(Email.Create(notification.Email),
                    PersonName.ParseFromFullName(notification.EmployeeName), null, null), cancellationToken);

                await _employeeRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            else if (notification.EventType == EmployeeEventType.Dismissal)
            {
                var employee =
                    await _employeeRepository.FindByEmailAsync(notification.Email, cancellationToken);
                if (employee != null)
                {
                    await _employeeRepository.DeleteAsync(employee, cancellationToken);
                    await _employeeRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                    return;
                }
            }
        }
    }
}