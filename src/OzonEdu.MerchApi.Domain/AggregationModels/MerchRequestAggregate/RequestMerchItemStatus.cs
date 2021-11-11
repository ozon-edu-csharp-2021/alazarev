using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate
{
    public class RequestMerchItemStatus : EnumerationWithDescription
    {
        public static RequestMerchItemStatus NotVerified = new(1, "NotVerified", "Наличие на складе не проверено");
        public static RequestMerchItemStatus InStock = new(2, "InStock", "Есть на складе");
        public static RequestMerchItemStatus WaitingForSupply = new(3, "WaitingForSupply", "В ожидании поставки");

        public RequestMerchItemStatus(int id, string name, string description) : base(id, name, description)
        {
        }
    }
}