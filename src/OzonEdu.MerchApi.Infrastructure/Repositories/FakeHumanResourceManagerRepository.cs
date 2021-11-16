using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.HumanResourceManagerAggregate;
using OzonEdu.MerchApi.Domain.Contracts;

namespace OzonEdu.MerchApi.Infrastructure.Repositories
{
    public class FakeHumanResourceManagerRepository : FakeRepository<HumanResourceManager>,
        IHumanResourceManagerRepository
    {
        static FakeHumanResourceManagerRepository()
        {
            Items.Add(new HumanResourceManager(2, new EmployeeId(2)));
            Items.Add(new HumanResourceManager(1, new EmployeeId(3)));
        }

        public Task<IEnumerable<HumanResourceManager>> GetAll(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Items.AsEnumerable());
        }

        public FakeHumanResourceManagerRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}