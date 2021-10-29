using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.HttpModels;
using OzonEdu.MerchApi.Services.Interfaces;

namespace OzonEdu.MerchApi.Services
{
    /// <summary>
    /// Мерч-сервис. Реализация <see cref="IMerchService"/>>
    /// </summary>
    public class MerchService : IMerchService
    {
        public Task<RequestMerchResponse> RequestMerchAsync(RequestMerchRequest request,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new RequestMerchResponse(RequestMerchStatus.Reserved));
        }

        public Task<GetReceivingMerchInfoResponse> GetReceivingMerchInfoAsync(GetReceivingMerchInfoRequest request,
            CancellationToken cancellationToken = default)
        {
            var response = new[]
            {
                new MerchInfo(MerchType.WelcomePack, DateTime.UtcNow.AddDays(-40)),
                new MerchInfo(MerchType.ConferenceListenerPack, DateTime.UtcNow.AddDays(-10)),
                new MerchInfo(MerchType.ProbationPeriodEndingPack, DateTime.UtcNow.AddDays(-500))
            };

            return Task.FromResult(new GetReceivingMerchInfoResponse(response));
        }
    }
}