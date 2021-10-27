using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OzonEdu.MerchApi.Infrastructure.Middlewares
{
    /// <summary>
    /// Терминальный middleware для получения версии
    /// </summary>
    public sealed class VersionMiddleware
    {
        public VersionMiddleware(RequestDelegate next)
        {
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await context.Response.WriteAsJsonAsync(new
            {
                Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown version",
                ServiceName = nameof(MerchApi)
            });
        }
    }
}