using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OzonEdu.MerchApi.Domain.Contracts.EmployeesService;
using OzonEdu.MerchApi.Grpc;
using OzonEdu.MerchApi.Infrastructure.Configuration;

namespace OzonEdu.MerchApi.Infrastructure.InfrastructureServices
{
    public class EmployeeApiService : IEmployeeApiService
    {
        public const string HttpClientName = "EmployeeApi";
        private const string GetByIdUrl = "api/employees/{0}";
        private const string GetAllUrl = "api/employees";
        private readonly HttpClient _httpClient;

        public EmployeeApiService(IHttpClientFactory clientFactory, IOptions<EmployeeServiceOptions> options)
        {
            _httpClient = clientFactory.CreateClient(HttpClientName);
            _httpClient.BaseAddress = new Uri(options.Value.Url);
        }

        public async Task<Employee> GetByIdAsync(int id, CancellationToken token = default)
        {
            var response =
                await _httpClient.GetFromJsonAsync<Employee>(
                    string.Format(GetByIdUrl, id.ToString()),
                    token);
            return response;
        }

        public async Task<Employee[]> GetAllAsync(CancellationToken token = default)
        {
            var response =
                await _httpClient.GetFromJsonAsync<GetAllEmployeesResponse>(GetAllUrl, token);
            return response?.Items ?? System.Array.Empty<Employee>();
        }

        private record GetAllEmployeesResponse
        {
            public Employee[] Items { get; set; }
        }
    }
}