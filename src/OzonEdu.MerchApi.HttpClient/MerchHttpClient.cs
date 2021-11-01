using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.HttpModels;
using SystemHttpClient = System.Net.Http.HttpClient;

namespace OzonEdu.MerchApi.HttpClient
{
    public sealed class MerchHttpClient : IMerchHttpClient
    {
        public const string HttpClientName = "MerchApi";
        private const string RequestMerchRequestUrl = "v1/api/merch/request";
        private const string GetReceivingMerchInfoRequestUrl = "v1/api/merch/getinfo/{0}";

        private readonly SystemHttpClient _httpClient;

        public MerchHttpClient(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient(HttpClientName);
        }

        public async Task<RequestMerchResponse> RequestMerch(RequestMerchRequest request,
            CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.PostAsJsonAsync(RequestMerchRequestUrl, request, cancellationToken);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<RequestMerchResponse>(body);
        }

        public async Task<GetReceivingMerchInfoResponse> GetReceivingMerchInfo(GetReceivingMerchInfoRequest request,
            CancellationToken cancellationToken = default)
        {
            var response =
                await _httpClient.GetFromJsonAsync<GetReceivingMerchInfoResponse>(
                    string.Format(GetReceivingMerchInfoRequestUrl, request.EmployeeId),
                    cancellationToken);
            return response;
        }
    }
}