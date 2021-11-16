using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate
{
    public sealed class MerchCategory : Enumeration
    {
        public static MerchCategory TShirt = new(1, nameof(TShirt));
        public static MerchCategory Sweatshirt = new(2, nameof(Sweatshirt));
        public static MerchCategory Notepad = new(3, nameof(Notepad));
        public static MerchCategory Bag = new(4, nameof(Bag));
        public static MerchCategory Pen = new(5, nameof(Pen));
        public static MerchCategory Socks = new(6, nameof(Socks));

        public MerchCategory(int id, string name) : base(id, name)
        {
        }
    }
}