using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;

namespace OzonEdu.MerchApi.Infrastructure.Persistence.Models
{
    public class MerchItemDto
    {
        public NameDto Name { get; set; }
        public SkuDto Sku { get; set; }
        public MerchCategoryDto Category { get; set; }
    }
}