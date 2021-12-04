using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Contracts.EmployeesService
{
    public interface IEmployeeApiService
    {
        Task<Employee> GetByIdAsync(int id, CancellationToken token = default);
        Task<Employee[]> GetAllAsync(CancellationToken token = default);
    }
}