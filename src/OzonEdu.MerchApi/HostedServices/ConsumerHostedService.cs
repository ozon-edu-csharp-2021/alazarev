using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Confluent.Kafka;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Events;
using OzonEdu.MerchApi.Infrastructure.Configuration;

namespace OzonEdu.MerchApi.HostedServices
{
    public class ConsumerHostedService : BackgroundService
    {
        private readonly KafkaOptions _kafkaOptions;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<ConsumerHostedService> _logger;

        public ConsumerHostedService(
            IOptions<KafkaOptions> kafkaOptions,
            IServiceScopeFactory scopeFactory,
            ILogger<ConsumerHostedService> logger, IMapper mapper)
        {
            _kafkaOptions = kafkaOptions.Value;
            _scopeFactory = scopeFactory;
            _logger = logger;
            _mapper = mapper;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = _kafkaOptions.GroupId,
                BootstrapServers = _kafkaOptions.BootstrapServers,
                AllowAutoCreateTopics = true,
                EnableAutoCommit = false,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(new[] { _kafkaOptions.EmployeesServiceTopic, _kafkaOptions.StockApiTopic });
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    using var scope = _scopeFactory.CreateScope();
                    try
                    {
                        await Task.Yield();
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        var consumeResult = consumer.Consume(cancellationToken);
                        if (consumeResult != null)
                        {
                            if (consumeResult.Topic == _kafkaOptions.EmployeesServiceTopic)
                            {
                                var message =
                                    JsonSerializer.Deserialize<NotificationEvent>(consumeResult.Message.Value);
                                var employeeNotification = _mapper.Map<EmployeeNotificationDomainEvent>(message);
                                await mediator.Publish(employeeNotification, cancellationToken);
                                consumer.Commit(consumeResult);
                            }
                            else if (consumeResult.Topic == _kafkaOptions.StockApiTopic)
                            {
                                var message =
                                    JsonSerializer.Deserialize<StockReplenishedEvent>(consumeResult.Message.Value);
                                var newStockReplenishedEvent =
                                    new NewStockReplenishedEvent(message.Type.Select(t => new Sku(t.Sku)));
                                await mediator.Publish(newStockReplenishedEvent, cancellationToken);
                                consumer.Commit(consumeResult);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error while get consume. Message {ex.Message}");
                    }
                }
            }
            finally
            {
                consumer.Commit();
                consumer.Close();
            }
        }
    }
}