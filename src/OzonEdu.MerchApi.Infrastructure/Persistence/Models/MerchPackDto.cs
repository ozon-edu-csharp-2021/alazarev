using EmailValueObject = OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects.Email;
using ClothingSizeEnum = OzonEdu.MerchApi.Domain.AggregationModels.Enums.ClothingSize;

namespace OzonEdu.MerchApi.Infrastructure.Persistence.Models
{
    public class MerchPackDto
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Positions { get; set; }
    }
}