using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using CSharpCourse.Core.Lib.Events;
using CSharpCourse.Core.Lib.Models;
using MediatR;
using OzonEdu.MerchApi.Domain.Contracts.EmployeesService;
using OzonEdu.MerchApi.Domain.Events;
using OzonEdu.MerchApi.Infrastructure.Bus;

namespace OzonEdu.MerchApi.Infrastructure.Handlers.DomainEvents
{
    public class RequestReservedEventHandler : INotificationHandler<RequestReservedEvent>
    {
        private readonly IProducerBuilderWrapper _producerBuilderWrapper;
        private readonly IEmployeeApiService _employeeApiService;

        public RequestReservedEventHandler(IProducerBuilderWrapper producerBuilderWrapper,
            IEmployeeApiService employeeApiService)
        {
            _producerBuilderWrapper = producerBuilderWrapper;
            _employeeApiService = employeeApiService;
        }

        public async Task Handle(RequestReservedEvent notification, CancellationToken cancellationToken)
        {
            var employee =
                await _employeeApiService.GetByIdAsync(notification.Request.EmployeeId.Value, cancellationToken);
            if (employee == null)
                throw new Exception("Employee not found");

            await _producerBuilderWrapper.Producer.ProduceAsync(_producerBuilderWrapper.EmailTopic,
                new Message<string, string>()
                {
                    Key = notification.Request.Id.ToString(),
                    Value = JsonSerializer.Serialize(new NotificationEvent()
                    {
                        EmployeeEmail = employee.Email,
                        ManagerEmail = notification.Request.ManagerEmail.Value,
                        EmployeeName = $"{employee.FirstName} {employee.LastName}"
                    })
                }, cancellationToken);
        }
    }
}