using CSharpCourse.Core.Lib.Enums;
using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;

namespace OzonEdu.MerchApi.Infrastructure.Commands.CreateMerchRequest
{
    public class CreateMerchRequestCommand : IRequest<CreateMerchRequestResult>
    {
        public string EmployeeEmail { get; }
        public string ManagerEmail { get; }
        public MerchType MerchType { get; }
        public MerchRequestMode MerchRequestMode { get; }

        public CreateMerchRequestCommand(string employeeEmail, string managerEmail, MerchType merchType,
            MerchRequestMode merchRequestMode)
        {
            EmployeeEmail = employeeEmail;
            MerchType = merchType;
            MerchRequestMode = merchRequestMode;
            ManagerEmail = managerEmail;
        }
    }
}