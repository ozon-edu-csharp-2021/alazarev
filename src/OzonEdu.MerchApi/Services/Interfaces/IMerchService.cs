using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.Models;
using MerchType = CSharpCourse.Core.Lib.Enums.MerchType;

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
        /// <param name="employeeId">Идентфиикатор сотрудника</param>
        /// <param name="merchType">Тип запрашиваемого мерча</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RequestMerchStatus> RequestMerchAsync(int employeeId, MerchType merchType,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить информацию по всем выданным мерчам
        /// </summary>
        /// <param name="employeeId">Идентфиикатор сотрудника</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<MerchInfoModel>> GetReceivingMerchInfoAsync(int employeeId,
            CancellationToken cancellationToken = default);
    }
}