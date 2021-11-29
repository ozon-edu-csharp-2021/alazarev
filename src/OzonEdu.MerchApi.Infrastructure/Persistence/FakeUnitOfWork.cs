using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.Domain.Contracts;

namespace OzonEdu.MerchApi.Infrastructure.Persistence
{
    public class FakeUnitOfWork : IUnitOfWork
    {
        public ValueTask StartTransaction(CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
           return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}