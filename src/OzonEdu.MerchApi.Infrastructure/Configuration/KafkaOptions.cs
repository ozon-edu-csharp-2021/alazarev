namespace OzonEdu.MerchApi.Infrastructure.Configuration
{
    public class KafkaOptions
    {
        public string GroupId { get; set; }

        public string StockApiTopic { get; set; }
        public string EmployeesServiceTopic { get; set; }
        public string EmailTopic { get; set; }
        public string BootstrapServers { get; set; }
    }
}