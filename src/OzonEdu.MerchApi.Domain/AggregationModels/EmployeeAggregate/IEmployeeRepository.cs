using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.Domain.Contracts;

namespace OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee> FindByEmailAsync(Email email, CancellationToken cancellationToken = default);
    }
}