using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Enums;
using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Events;
using OzonEdu.MerchApi.Infrastructure.Commands.CreateMerchRequest;

namespace OzonEdu.MerchApi.Infrastructure.Handlers.DomainEvents
{
    public class EmployeeNotificationDomainEventHandler : INotificationHandler<EmployeeNotificationDomainEvent>
    {
        private readonly IMerchRequestRepository _merchRequestRepository;
        private readonly IMediator _mediator;

        public EmployeeNotificationDomainEventHandler(IMerchRequestRepository merchRequestRepository,
            IMediator mediator)
        {
            _merchRequestRepository = merchRequestRepository;
            _mediator = mediator;
        }

        public async Task Handle(EmployeeNotificationDomainEvent notification, CancellationToken cancellationToken)
        {
            if (notification.MerchDeliveryPayload != null)
            {
                var createMerchRequestCommand =
                    new CreateMerchRequestCommand(notification.EmployeeEmail, notification.ManagerEmail,
                        notification.MerchDeliveryPayload.ClothingSize, notification.MerchDeliveryPayload.MerchType,
                        MerchRequestMode.Auto);

                await _mediator.Send(createMerchRequestCommand, cancellationToken);
            }
        }
    }
}