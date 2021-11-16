using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OzonEdu.MerchApi.GrpcServices;
using OzonEdu.MerchApi.Infrastructure;
using OzonEdu.MerchApi.Infrastructure.Extensions;
using OzonEdu.MerchApi.Mappers;

namespace OzonEdu.MerchApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(MerchApiInfrastructure.Marker);
            services.AddAutoMapper(cfg => cfg.AddProfile<GprcMappingProfile>());
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