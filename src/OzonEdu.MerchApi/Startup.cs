using AutoMapper.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OzonEdu.MerchApi.GrpcServices;
using OzonEdu.MerchApi.Infrastructure;
using OzonEdu.MerchApi.Infrastructure.Configuration;
using OzonEdu.MerchApi.Infrastructure.Extensions;
using OzonEdu.MerchApi.Mappers;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace OzonEdu.MerchApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<MerchApiGrpcService>();
                endpoints.MapControllers();
            });
        }
    }
}