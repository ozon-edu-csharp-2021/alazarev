namespace OzonEdu.MerchApi.HttpModels
{
    /// <summary>
    /// Результат запроса на получение мерча
    /// </summary>
    public enum RequestMerchStatus
    {
        //Зарезервировано
        Reserved = 0,
        //Уже получил
        AlreadyReceived = 1,
        //нет в наличии
        NotAvailable = 2
    }
}