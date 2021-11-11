using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace OzonEdu.MerchApi.Infrastructure.Middlewares
{
    /// <summary>
    /// Терминальный middleware для получения версии
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            LogRequestRoute(context.Request);
            LogRequestHeader(context.Request);
            await LogRequestBodyAsync(context.Request);
            await LogResponseBodyAsync(context);
        }

        /// <summary>
        /// Логируем адрес запроса
        /// </summary>
        /// <param name="request">Запрос</param>
        private void LogRequestRoute(HttpRequest request)
        {
            _logger.LogInformation(
                $"[Request route]: {request.Scheme}://{request.Host}{request.Path}{request.QueryString}");
        }

        /// <summary>
        /// Логируем заголовок запроса
        /// </summary>
        /// <param name="request">Запрос</param>
        private void LogRequestHeader(HttpRequest request)
        {
            var headerAsText = request.Headers.Aggregate(new StringBuilder($"[Request header]: {Environment.NewLine}"),
                (sb, h) =>
                {
                    sb.AppendLine($"\t\t{h.Key} - {h.Value}");
                    return sb;
                }).ToString();

            _logger.LogInformation(headerAsText);
        }

        /// <summary>
        /// Логируем тело запроса
        /// </summary>
        /// <param name="request">Запрос</param>
        private async Task LogRequestBodyAsync(HttpRequest request)
        {
            try
            {
                request.EnableBuffering();
                var buffer = ArrayPool<byte>.Shared.Rent(Convert.ToInt32(request.ContentLength));
                await request.Body.ReadAsync(buffer, 0, buffer.Length);
                var bodyAsText = Encoding.UTF8.GetString(buffer);
                ArrayPool<byte>.Shared.Return(buffer);
                request.Body.Seek(0, SeekOrigin.Begin);
                _logger.LogInformation($"[Request body]: {bodyAsText}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Couldn't log request body");
            }
        }

        /// <summary>
        /// Логируем ответ
        /// </summary>
        /// <param name="context">Контекст</param>
        private async Task LogResponseBodyAsync(HttpContext context)
        {
            try
            {
                //сохраняем текущий стрим
                var currentResponseStream = context.Response.Body;
                await using var responseStream = _recyclableMemoryStreamManager.GetStream();
                //меняем стрим ответа на наш стрим
                context.Response.Body = responseStream;
                //получаем ответ
                await _next(context);
                //перематываем стрим на начало
                responseStream.Seek(0, SeekOrigin.Begin);
                var buffer = ArrayPool<byte>.Shared.Rent(Convert.ToInt32(context.Response.Body.Length));
                await context.Response.Body.ReadAsync(buffer, 0, buffer.Length);
                var bodyAsText = Encoding.UTF8.GetString(buffer);
                ArrayPool<byte>.Shared.Return(buffer);
                //перематываем стрим на начало еще раз
                responseStream.Seek(0, SeekOrigin.Begin);
                _logger.LogInformation($"[Response body]: {bodyAsText}");
                //в текущий стрим помещаем стрим с ответом
                await responseStream.CopyToAsync(currentResponseStream);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Couldn't log response body");
            }
        }
    }
}