using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OzonEdu.MerchApi.Infrastructure.Middlewares
{
    /// <summary>
    /// Терминальный middleware для статуса готовности
    /// </summary>
    public sealed class ReadyMiddleware
    {
        public ReadyMiddleware(RequestDelegate next)
        {
        }

        public Task InvokeAsync(HttpContext context)=> Task.CompletedTask;
    }
}