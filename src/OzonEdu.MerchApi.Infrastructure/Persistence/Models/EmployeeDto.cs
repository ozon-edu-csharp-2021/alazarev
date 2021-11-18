using System.Diagnostics.CodeAnalysis;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using EmailValueObject = OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects.Email;
using ClothingSizeEnum = OzonEdu.MerchApi.Domain.AggregationModels.Enums.ClothingSize;

namespace OzonEdu.MerchApi.Infrastructure.Persistence.Models
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? ClothingSize { get; set; }
        public double? Height { get; set; }
    }
}