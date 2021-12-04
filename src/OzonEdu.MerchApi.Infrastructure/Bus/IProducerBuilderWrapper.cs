using Confluent.Kafka;

namespace OzonEdu.MerchApi.Infrastructure.Bus
{
    public interface IProducerBuilderWrapper
    {
        /// <summary>
        /// Producer instance
        /// </summary>
        IProducer<string, string> Producer { get; set; }
        
        string EmailTopic { get; set; }
    }
}