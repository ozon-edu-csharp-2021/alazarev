using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.Domain.Contracts;

namespace OzonEdu.MerchApi.Infrastructure.Uow
{
    public class FakeUnitOfWork : IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }

        public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }
    }
}