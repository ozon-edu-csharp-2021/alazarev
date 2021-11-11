using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.Domain.Contracts.StockApiService;

namespace OzonEdu.MerchApi.Infrastructure.InfrastructureServices
{
    public class FakeStockApiService : IStockApiService
    {
        public Task<IEnumerable<StockItem>> CheckStockItemsAsync(IEnumerable<long> skus,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(skus.Select(id => new StockItem() {SkuId = id, Quantity = 99}));
        }

        public Task<bool> ReserveStockItemsAsync(IEnumerable<long> skus, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }
    }
}