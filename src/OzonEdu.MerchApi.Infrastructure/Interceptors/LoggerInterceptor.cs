using System.Text.Json;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace OzonEdu.MerchApi.Infrastructure.Interceptors
{
    /// <summary>
    /// Интерсептер для логгирования request и response обычных unary вызовов
    /// </summary>
    public sealed class LoggerInterceptor : Interceptor
    {
        private readonly ILogger<LoggerInterceptor> _logger;

        public LoggerInterceptor(ILogger<LoggerInterceptor> logger)
        {
            _logger = logger;
        }

        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var requestJson = JsonSerializer.Serialize(request);
            _logger.LogInformation($"[Request GRPC]: {requestJson}");

            var response = base.UnaryServerHandler(request, context, continuation);

            var responseJson = JsonSerializer.Serialize(response);
            _logger.LogInformation($"[Response GRPC]: {responseJson}");

            return response;
        }
    }
}