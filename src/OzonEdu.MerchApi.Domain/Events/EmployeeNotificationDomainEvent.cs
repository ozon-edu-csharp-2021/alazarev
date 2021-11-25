using CSharpCourse.Core.Lib.Enums;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;

namespace OzonEdu.MerchApi.Domain.Events
{
    public class EmployeeNotificationDomainEvent : INotification
    {
        public string EmployeeEmail { get; set; }
        public string ManagerEmail { get; set; }
        public string EmployeeName { get; set; }

        public EmployeeEventType EventType { get; set; }

        public ClothingSize ClothingSize { get; set; }
        public MerchDeliveryEventPayload MerchDeliveryPayload { get; set; }
    }
}