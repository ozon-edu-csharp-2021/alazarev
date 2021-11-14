using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Contracts.StockApiService
{
    /// <summary>
    /// Интерфейс для работы с сток апи. Является ACL реализацией
    /// </summary>
    public interface IStockApiService
    {
        /// <summary>
        ///  проверить наличие по коллекции sku
        /// </summary>
        /// <param name="skus"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<StockItem>> GetStockItemsAvailabilityAsync(IEnumerable<long> skus,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// зарезервировать по коллекции ску
        /// </summary>
        /// <param name="skus"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> ReserveStockItemsAsync(IEnumerable<long> skus, CancellationToken cancellationToken = default);
    }
}