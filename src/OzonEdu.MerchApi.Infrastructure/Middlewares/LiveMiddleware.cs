using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OzonEdu.MerchApi.Infrastructure.Middlewares
{
    /// <summary>
    /// Терминальный middleware для health check'a
    /// </summary>
    public sealed class LiveMiddleware
    {
        public LiveMiddleware(RequestDelegate next)
        {
        }

        public Task InvokeAsync(HttpContext context) => Task.CompletedTask;
    }
}