using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;

namespace OzonEdu.MerchApi.Domain.Events
{
    public class MerchInStockEvent : INotification
    {
        public MerchInStockEvent(EmployeeId employeeId, MerchRequestId requestId)
        {
            EmployeeId = employeeId;
            RequestId = requestId;
        }

        public EmployeeId EmployeeId { get; }
        public MerchRequestId RequestId { get; }
    }
}