using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.Contracts.StockApiService;
using OzonEdu.MerchApi.Domain.Events;
using OzonEdu.MerchApi.Infrastructure.Configuration;

namespace OzonEdu.MerchApi.Infrastructure.Handlers.DomainEvents
{
    public class NewSupplyArrivedEventHandler : INotificationHandler<NewSupplyArrivedEvent>
    {
        private readonly IMerchRequestRepository _merchRequestRepository;
        private readonly IStockApiService _stockApiService;
        private readonly StockApiOptions _stockApiConcurrencyOptions;

        public NewSupplyArrivedEventHandler(IMerchRequestRepository merchRequestRepository,
            IStockApiService stockApiService, IOptions<StockApiOptions> stockApiConcurrencyOptions)
        {
            _merchRequestRepository = merchRequestRepository;
            _stockApiService = stockApiService;
            _stockApiConcurrencyOptions = stockApiConcurrencyOptions.Value;
        }

        public async Task Handle(NewSupplyArrivedEvent notification, CancellationToken cancellationToken)
        {
            //нужно обрабатывать запросы последовательно один за другим
            //в тех дол. нужно через очередь или channel, но уже ночь :()

            await Task.Delay(TimeSpan.FromMinutes(3));

            var merchRequests =
                await _merchRequestRepository.GetAllWaitingForSupplyRequestsAsync(cancellationToken);

            var skus = notification.Items.Select(i => i.Sku);

            var merchRequestForChecking = merchRequests
                .Where(r => r.Items.Any(i => skus.Contains(i.Sku)));
            using var semaphore = new SemaphoreSlim(_stockApiConcurrencyOptions.MaxRequestsCount);

            var tasks = merchRequestForChecking.Select(async request =>
            {
                await semaphore.WaitAsync();
                var isAvailability =
                    await _stockApiService.CheckMerchRequestAvailabilityAsync(request, cancellationToken);

                if (isAvailability)
                {
                    if (Equals(request.Mode, MerchRequestMode.ByRequest))
                    {
                        request.Complete(false);
                    }
                    else
                    {
                        var result = await _stockApiService.ReserveStockItemsAsync(request, cancellationToken);
                        request.Complete(result, DateTimeOffset.UtcNow);
                    }

                    await _merchRequestRepository.UpdateAsync(request, cancellationToken);
                }

                semaphore.Release();
            });

            await Task.WhenAll(tasks);
        }
    }
}