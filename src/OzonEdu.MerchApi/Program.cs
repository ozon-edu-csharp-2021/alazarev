using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using OzonEdu.MerchApi.Infrastructure.Extensions;

namespace OzonEdu.MerchApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(options =>
                    {
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                        {
                            // options.ListenLocalhost(5001, o => o.Protocols =
                            //     HttpProtocols.Http1AndHttp2);
                        }
                    });
                })
                .AddInfrastructure();
    }
}