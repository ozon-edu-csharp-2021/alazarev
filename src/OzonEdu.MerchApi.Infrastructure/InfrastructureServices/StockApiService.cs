using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpCourse.Core.Lib.Enums;
using Google.Protobuf.Collections;
using Microsoft.Extensions.Options;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.Contracts.StockApiService;
using OzonEdu.MerchApi.Infrastructure.Configuration;
using OzonEdu.MerchApi.Infrastructure.StockApi;
using StockItemUnit = OzonEdu.MerchApi.Domain.Contracts.StockApiService.StockItemUnit;

namespace OzonEdu.MerchApi.Infrastructure.InfrastructureServices
{
    public class StockApiService : IStockApiService
    {
        private readonly StockApiOptions _stockApiConcurrencyOptions;

        private readonly StockApiGrpc.StockApiGrpcClient _stockApiGrpcClient;
        private readonly IMapper _mapper;

        public StockApiService(
            IOptions<StockApiOptions> stockApiConcurrencyOptions, IMapper mapper,
            StockApiGrpc.StockApiGrpcClient stockApiGrpcClient)
        {
            _mapper = mapper;
            _stockApiGrpcClient = stockApiGrpcClient;
            _stockApiConcurrencyOptions = stockApiConcurrencyOptions.Value;
        }

        public async Task<bool> CheckMerchRequestAvailabilityAsync(MerchRequest merchRequest,
            CancellationToken cancellationToken = default)
        {
            var skusRequest = new SkusRequest();
            skusRequest.Skus.AddRange(merchRequest.Items.Select(i => i.Sku.Value));
            var response = await _stockApiGrpcClient.GetStockItemsAvailabilityAsync(skusRequest);
            
            return merchRequest.Items.All(r =>
            {
                var stockItem = response.Items.FirstOrDefault(i => i.Sku == r.Sku.Value);

                return stockItem != null && stockItem.Quantity >= r.Quantity.Value;
            });
        }


        public async Task<bool> ReserveStockItemsAsync(MerchRequest merchRequest,
            CancellationToken cancellationToken = default)
        {
            var request = new GiveOutItemsRequest();
            request.Items.AddRange(merchRequest.Items.Select(i => new SkuQuantityItem()
            {
                Sku = i.Sku.Value,
                Quantity = i.Quantity.Value
            }));

            var response = await _stockApiGrpcClient.GiveOutItemsAsync(request);

            return response.Result == GiveOutItemsResponse.Types.Result.Successful;
        }

        public async Task<IEnumerable<StockItemUnit>> GetSkusByMerchPackAndSizeAsync(MerchPack merchPack,
            ClothingSize clothingSize,
            CancellationToken cancellationToken = default)
        {
            using var semaphore = new SemaphoreSlim(_stockApiConcurrencyOptions.MaxRequestsCount);
            var tasks = merchPack.Positions.Select(async p =>
            {
                await semaphore.WaitAsync();
                var response = await _stockApiGrpcClient.GetByItemTypeAsync(new IntIdModel
                {
                    Id = (int)p.ItemId.Value
                }, cancellationToken: cancellationToken);
                semaphore.Release();

                var itemBySize = response.Items.FirstOrDefault(i => i.SizeId == (long)clothingSize);
                return itemBySize ?? response.Items.FirstOrDefault();
            });

            await Task.WhenAll(tasks);

            return _mapper.Map<IEnumerable<StockItemUnit>>(tasks.Select(t => t.Result));
        }
    }
}