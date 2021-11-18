using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Infrastructure.Persistence.Repositories;

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

        public Task<Employee> FindByEmailAsync(Email email, CancellationToken cancellationToken = default)
            => Task.FromResult(Items.FirstOrDefault(e => e.Email.Equals(email)));

        public Task<Employee> CreateAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<Employee> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public FakeEmployeeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}