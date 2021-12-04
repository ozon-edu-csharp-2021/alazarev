using System;
using CSharpCourse.Core.Lib.Enums;
using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.Contracts.EmployeesService;

namespace OzonEdu.MerchApi.Infrastructure.Commands.CreateMerchRequest
{
    public class CreateMerchRequestCommand : IRequest<CreateMerchRequestResult>
    {
        public int? EmployeeId { get; }
        public Employee Employee { get; }
        public string ManagerEmail { get; }
        public MerchType MerchType { get; }
        public MerchRequestMode MerchRequestMode { get; }

        public CreateMerchRequestCommand(int employeeId, string managerEmail,
            MerchType merchType,
            MerchRequestMode merchRequestMode)
        {
            EmployeeId = employeeId;
            MerchType = merchType;
            MerchRequestMode = merchRequestMode;
            ManagerEmail = managerEmail;
        }

        public CreateMerchRequestCommand(Employee employee, string managerEmail,
            MerchType merchType,
            MerchRequestMode merchRequestMode)
        {
            Employee = employee ?? throw new ArgumentNullException(nameof(employee));
            MerchType = merchType;
            MerchRequestMode = merchRequestMode;
            ManagerEmail = managerEmail;
        }
    }
}