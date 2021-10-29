using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.HttpModels;

namespace OzonEdu.MerchApi.Services.Interfaces
{
    /// <summary>
    /// Интерфейс мерчант-сервиса
    /// </summary>
    public interface IMerchService
    {
        /// <summary>
        /// Отправить запрос на выдачу мерча
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RequestMerchResponse> RequestMerchAsync(RequestMerchRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить информацию по всем выданным мерчам
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<GetReceivingMerchInfoResponse> GetReceivingMerchInfoAsync(GetReceivingMerchInfoRequest request,
            CancellationToken cancellationToken = default);
    }
}