using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.Contracts;

namespace OzonEdu.MerchApi.Infrastructure.Repositories
{
    public class FakeEmployeeRepository : FakeRepository<Employee>, IEmployeeRepository
    {
        static FakeEmployeeRepository()
        {
            Items.Add(new Employee(1, Email.Create("aleksey.lazarev@hotmail.com"),
                PersonName.Create("Aleksey", "Lazarev"), null,
                null));
            Items.Add(new Employee(2, Email.Create("denis.denisov@coldmail.com"), PersonName.Create("Denis", "Denisov"),
                null,
                null));
            Items.Add(new Employee(3, Email.Create("hard@manager.com"), PersonName.Create("Hard", "Manager"), null,
                null));
        }

        public Task<Employee> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
            => Task.FromResult(Items.FirstOrDefault(e => e.Email.Value.Equals(email)));

        public FakeEmployeeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}