using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Infrastructure.Repositories
{
    public class FakeRepository<TAggregationRoot> : IRepository<TAggregationRoot> where TAggregationRoot : IEntity
    {
        public static readonly IList<TAggregationRoot> Items = new List<TAggregationRoot>();

        public FakeRepository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork { get; }

        public Task<TAggregationRoot> CreateAsync(TAggregationRoot itemToCreate,
            CancellationToken cancellationToken = default)
        {
            Items.Add(itemToCreate);
            return Task.FromResult<TAggregationRoot>(itemToCreate);
        }

        public Task<TAggregationRoot> UpdateAsync(TAggregationRoot itemToUpdate,
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

        public Task<TAggregationRoot> GetAsync(object id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Items.FirstOrDefault(e => e.Id == (int)id));
        }

        public Task DeleteAsync(TAggregationRoot entity, CancellationToken cancellationToken = default)
        {
            Items.Remove(entity);
            return Task.CompletedTask;
        }
    }
}