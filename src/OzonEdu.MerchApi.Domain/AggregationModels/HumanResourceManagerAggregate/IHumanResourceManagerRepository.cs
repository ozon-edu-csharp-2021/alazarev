using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.Domain.Contracts;

namespace OzonEdu.MerchApi.Domain.AggregationModels.HumanResourceManagerAggregate
{
    public interface IHumanResourceManagerRepository : IRepository<HumanResourceManager>
    {
        public Task<IEnumerable<HumanResourceManager>> GetAll(CancellationToken cancellationToken = default);
    }
}