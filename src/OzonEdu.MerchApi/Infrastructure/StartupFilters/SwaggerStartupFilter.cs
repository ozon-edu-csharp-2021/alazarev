using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace OzonEdu.MerchApi.Infrastructure.StartupFilters
{
    /// <summary>
    /// Startup Filter для swagger'a
    /// </summary>
    public sealed class SwaggerStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                next(app);
            };
        }
    }
}