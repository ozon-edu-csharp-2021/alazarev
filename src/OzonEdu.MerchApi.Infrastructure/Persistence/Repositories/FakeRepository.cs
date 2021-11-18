using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Infrastructure.Persistence.Repositories
{
    public class FakeRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        public static readonly IList<TEntity> Items = new List<TEntity>();

        public FakeRepository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork { get; }

        // public Task<TEntity> CreateAsync(TEntity itemToCreate,
        //     CancellationToken cancellationToken = default)
        // {
        //     Items.Add(itemToCreate);
        //     return Task.FromResult<TEntity>(itemToCreate);
        // }

        public Task<TEntity> UpdateAsync(TEntity itemToUpdate,
            CancellationToken cancellationToken = default)
        {
            var findItem = Items.FirstOrDefault(e => e.Id == itemToUpdate.Id);
            if (findItem == null)
            {
                Items.Add(itemToUpdate);
            }
            else
            {
                var index = Items.IndexOf(findItem);
                Items[index] = itemToUpdate;
            }

            return Task.FromResult(itemToUpdate);
        }

        public Task<TEntity> GetAsync(object id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Items.FirstOrDefault(e => e.Id == (int)id));
        }
    }
}