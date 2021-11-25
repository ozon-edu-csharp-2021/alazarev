namespace OzonEdu.MerchApi.Domain.Contracts.StockApiService
{
    public class StockItemUnit
    {
        public long Sku { get; set; }
        public long ItemTypeId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public long? SizeId { get; set; }
    }
}