using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using OzonEdu.MerchApi.Infrastructure.Middlewares;

namespace OzonEdu.MerchApi.Infrastructure.StartupFilters
{
    /// <summary>
    /// Startup Filter для терминальных middlewares
    /// </summary>
    public sealed class TerminalStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.Map("/ready", builder => builder.UseMiddleware<ReadyMiddleware>());
                app.Map("/live", builder => builder.UseMiddleware<LiveMiddleware>());
                app.Map("/version", builder => builder.UseMiddleware<VersionMiddleware>());
                next(app);
            };
        }
    }
}