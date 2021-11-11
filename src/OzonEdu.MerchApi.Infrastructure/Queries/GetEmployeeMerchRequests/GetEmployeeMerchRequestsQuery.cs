using MediatR;

namespace OzonEdu.MerchApi.Infrastructure.Queries.GetEmployeeMerchRequests
{
    public class GetEmployeeMerchRequestsQuery : IRequest<GetEmployeeMerchRequestsResult>
    {
        public GetEmployeeMerchRequestsQuery(string employeeEmail)
        {
            EmployeeEmail = employeeEmail;
        }

        public string EmployeeEmail { get; }
    }
}