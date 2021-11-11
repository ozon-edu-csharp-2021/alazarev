using CSharpCourse.Core.Lib.Enums;
using CSharpCourse.Core.Lib.Events;
using MediatR;

namespace OzonEdu.MerchApi.Domain.Events
{
    public class EmployeeNotificationDomainEvent : INotification
    {
        public string Email { get; set; }
        
        public string EmployeeName { get; set; }
        
        public EmployeeEventType EventType { get; set; }
        
        public MerchDeliveryEventPayload MerchDeliveryPayload { get; set; }
    }
}