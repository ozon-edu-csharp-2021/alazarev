using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.HttpModels;

namespace OzonEdu.MerchApi.HttpClient
{
    public interface IMerchHttpClient
    {
        Task<RequestMerchResponseViewModel> RequestMerch(RequestMerchViewModel request,
            CancellationToken cancellationToken = default);

        Task<GetReceivingMerchInfoResponseViewModel> GetReceivingMerchInfo(GetReceivingMerchInfoViewModel request,
            CancellationToken cancellationToken = default);
    }
}