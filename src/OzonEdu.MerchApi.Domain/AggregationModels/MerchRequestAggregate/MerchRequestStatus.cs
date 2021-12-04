using System;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate
{
    public class MerchRequestStatus : EnumerationWithDescription
    {
        public static MerchRequestStatus Created = new(1, "Created", "Заявка создана");

        public static MerchRequestStatus InProcess =
            new(2, "InProcess", "Заявка готова к проверки позиций на складе");

        public static MerchRequestStatus WaitingForSupply = new(3, "WaitingForSupply", "В ожидании поставки");
        public static MerchRequestStatus Reserved = new(4, "Reserved", "Позиции зарезервированы");

        public static MerchRequestStatus Informed =
            new(5, "Informed", "Сотрудник был проинформирован о поступлении мерча");

        public static MerchRequestStatus Error =
            new(6, "Error", "Произошла ошибка");

        public static MerchRequestStatus Canceled =
            new(7, "Canceled", "Заявка отменена");

        public MerchRequestStatus(int id, string name, string description) : base(id, name, description)
        {
        }
    }
}