using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.HttpModels;

namespace OzonEdu.MerchApi.HttpClient
{
    public interface IMerchHttpClient
    {
        Task<RequestMerchResponse> RequestMerch(RequestMerchRequest request,
            CancellationToken cancellationToken = default);

        Task<GetReceivingMerchInfoResponse> GetReceivingMerchInfo(GetReceivingMerchInfoRequest request,
            CancellationToken cancellationToken = default);
    }
}