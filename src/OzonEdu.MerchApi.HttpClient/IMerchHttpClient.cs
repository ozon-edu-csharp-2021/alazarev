using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.HttpModels;

namespace OzonEdu.MerchApi.HttpClient
{
    public interface IMerchHttpClient
    {
        Task<RequestMerchResponse> RequestMerch(CreateMerchRequest request,
            CancellationToken cancellationToken = default);

        Task<GetMerchInfoResponse> GetMerchInfo(string employeeEmail,
            CancellationToken cancellationToken = default);
    }
}