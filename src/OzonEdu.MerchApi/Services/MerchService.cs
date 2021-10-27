using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.Models;
using OzonEdu.MerchApi.Services.Interfaces;
using MerchType = CSharpCourse.Core.Lib.Enums.MerchType;

namespace OzonEdu.MerchApi.Services
{
    /// <summary>
    /// Мерч-сервис. Реализация <see cref="IMerchService"/>>
    /// </summary>
    public class MerchService : IMerchService
    {
        public Task<RequestMerchStatus> RequestMerchAsync(int employeeId, MerchType merchType,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(RequestMerchStatus.Reserved);
        }

        public Task<IEnumerable<MerchInfoModel>> GetReceivingMerchInfoAsync(int employeeId,
            CancellationToken cancellationToken = default)
        {
            var response = new[]
            {
                new MerchInfoModel(MerchType.WelcomePack, DateTime.UtcNow.AddDays(-40)),
                new MerchInfoModel(MerchType.ConferenceListenerPack, DateTime.UtcNow.AddDays(-10)),
                new MerchInfoModel(MerchType.ProbationPeriodEndingPack, DateTime.UtcNow.AddDays(-500))
            };

            return Task.FromResult<IEnumerable<MerchInfoModel>>(response);
        }
    }
}