using System;
using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using OzonEdu.MerchApi.Infrastructure.Middlewares;

namespace OzonEdu.MerchApi.Infrastructure.StartupFilters
{
    /// <summary>
    /// Startup Filter для middleware, отвечающего за логирование запросов
    /// </summary>
    public sealed class RequestResponseLoggingStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) => app =>
        {
            // используем RequestResponseLoggingMiddleware, если не grpc-запрос
            // или не swagger. нет причин логировать html
            app.UseWhen(context => !"application/grpc".Equals(context.Request.ContentType) &&
                                   !(context.Request.Path.HasValue &&
                                     context.Request.Path.Value.StartsWith("/swagger")),
                builder => builder.UseMiddleware<RequestResponseLoggingMiddleware>());
            next(app);
        };
    }
}