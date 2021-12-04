using System;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using OzonEdu.MerchApi.Infrastructure.Configuration;

namespace OzonEdu.MerchApi.Infrastructure.Bus
{
    public class ProducerBuilderWrapper : IProducerBuilderWrapper
    {
        /// <inheritdoc cref="Producer"/>
        public IProducer<string, string> Producer { get; set; }
        
        public string EmailTopic { get; set; }


        public ProducerBuilderWrapper(IOptions<KafkaOptions> configuration)
        {
            var configValue = configuration.Value;
            if (configValue is null)
                throw new ApplicationException("Configuration for kafka server was not specified");

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = configValue.BootstrapServers
            };

            Producer = new ProducerBuilder<string, string>(producerConfig).Build();
            EmailTopic = configValue.EmailTopic;
        }
    }
}