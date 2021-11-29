using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;

namespace OzonEdu.MerchApi.Infrastructure.Persistence.Models
{
    public class MerchPackItemDto
    {
        public NameDto Name { get; set; }
        public ItemIdDto ItemId { get; set; }
        public QuantityDto Quantity { get; set; }
    }
}