using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OzonEdu.MerchApi.Infrastructure.Filters;
using OzonEdu.MerchApi.Infrastructure.Interceptors;
using OzonEdu.MerchApi.Infrastructure.StartupFilters;

namespace OzonEdu.MerchApi.Infrastructure.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddInfrastructure(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IStartupFilter, RequestResponseLoggingStartupFilter>();
                services.AddSingleton<IStartupFilter, TerminalStartupFilter>();
                services.AddSingleton<IStartupFilter, SwaggerStartupFilter>();
                services.AddGrpc(options => options.Interceptors.Add<LoggerInterceptor>());
                services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());


                services.AddControllersWithViews()
                    .AddJsonOptions(options =>
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo {Title = nameof(MerchApi), Version = "v1"});

                    options.CustomSchemaIds(x => x.FullName);
                    var xmlFileName = "OzonEdu.MerchApi.xml";
                    var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                    options.IncludeXmlComments(xmlFilePath);
                });
            }); 
            return builder;
        }
    }
}