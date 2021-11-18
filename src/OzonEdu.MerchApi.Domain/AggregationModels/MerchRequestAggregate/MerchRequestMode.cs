using System;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate
{
    public class MerchRequestMode : EnumerationWithDescription
    {
        public static MerchRequestMode ByRequest = new(1, "ByRequest", "Выдача мерча по запросу");
        public static MerchRequestMode Auto = new(2, "Auto", "Автоматическая выдача мерча сотруднику");

        public MerchRequestMode(int id, string name, string description) : base(id, name, description)
        {
        }

        public static MerchRequestMode Parse(int id) => id switch
        {
            1 => Auto,
            2 => Auto,
            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
        };
    }
}