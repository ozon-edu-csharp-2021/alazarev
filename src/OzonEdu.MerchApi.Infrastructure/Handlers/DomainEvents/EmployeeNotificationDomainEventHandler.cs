using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Enums;
using MediatR;
using OpenTracing;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Domain.Contracts.EmployeesService;
using OzonEdu.MerchApi.Domain.Events;
using OzonEdu.MerchApi.Infrastructure.Commands.CreateMerchRequest;

namespace OzonEdu.MerchApi.Infrastructure.Handlers.DomainEvents
{
    public class EmployeeNotificationDomainEventHandler : INotificationHandler<EmployeeNotificationDomainEvent>
    {
        private readonly IMerchRequestRepository _merchRequestRepository;
        private readonly IMediator _mediator;
        private readonly IEmployeeApiService _employeeApiService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITracer _tracer;

        public EmployeeNotificationDomainEventHandler(IMerchRequestRepository merchRequestRepository,
            IMediator mediator, IEmployeeApiService employeeApiService, IUnitOfWork unitOfWork, ITracer tracer)
        {
            _merchRequestRepository = merchRequestRepository;
            _mediator = mediator;
            _employeeApiService = employeeApiService;
            _unitOfWork = unitOfWork;
            _tracer = tracer;
        }

        public async Task Handle(EmployeeNotificationDomainEvent notification, CancellationToken cancellationToken)
        {
            using var span = _tracer.BuildSpan($"{nameof(EmployeeNotificationDomainEventHandler)}.Handle").StartActive();
            var employees = await _employeeApiService.GetAllAsync(cancellationToken);

            var employee = employees.FirstOrDefault(e =>
                !string.IsNullOrWhiteSpace(e.Email) && e.Email.Equals(notification.EmployeeEmail,
                    StringComparison.CurrentCultureIgnoreCase));

            if (employee == null) throw new Exception("Employee not found");

            if (notification.EventType == EmployeeEventType.Dismissal)
            {
                var requests =
                    await _merchRequestRepository.GetAllEmployeeRequestsAsync(employee.Id, cancellationToken);
                await _unitOfWork.StartTransaction(cancellationToken);
                foreach (var request in requests)
                {
                    request.Cancel();
                    await _merchRequestRepository.UpdateAsync(request, cancellationToken);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            if (notification.MerchDeliveryPayload != null)
            {
                var createMerchRequestCommand =
                    new CreateMerchRequestCommand(employee, notification.ManagerEmail,
                        notification.MerchDeliveryPayload.MerchType, MerchRequestMode.Auto);

                await _mediator.Send(createMerchRequestCommand, cancellationToken);
            }
        }
    }
}