using System;
using OzonEdu.MerchApi.HttpModels;
using OzonEdu.MerchApi.Models;

namespace OzonEdu.MerchApi.Mappers
{
    /// <summary>
    /// Маппер для работы с View Model
    /// </summary>
    public static class ViewModelMapper
    {
        public static Func<MerchInfoModel, MerchInfoViewModel> MerchInfoModelToViewModel =>
            m => new MerchInfoViewModel(m.MerchType, m.ReceivingDate);

        public static Func<RequestMerchStatus, RequestMerchViewModelStatus> RequestMerchStatusToViewModelStatus =>
            s => s switch
            {
                RequestMerchStatus.Reserved => RequestMerchViewModelStatus.Reserved,
                RequestMerchStatus.AlreadyReceived => RequestMerchViewModelStatus.AlreadyReceived,
                RequestMerchStatus.NotAvailable => RequestMerchViewModelStatus.AlreadyReceived,
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}