using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;

namespace OzonEdu.MerchApi.Domain.Contracts.StockApiService
{
    /// <summary>
    /// Интерфейс для работы с сток апи. Является ACL реализацией
    /// </summary>
    public interface IStockApiService
    {
        /// <summary>
        /// зарезервировать по коллекции ску
        /// </summary>
        /// <param name="skus"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> ReserveStockItemsAsync(MerchRequest merchRequest, CancellationToken cancellationToken = default);

        Task<bool> CheckMerchRequestAvailabilityAsync(MerchRequest merchRequest,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<StockItemUnit>> GetSkusByMerchPackAndSizeAsync(MerchPack merchPack, ClothingSize clothingSize,
            CancellationToken cancellationToken = default);
    }
}