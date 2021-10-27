using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using OzonEdu.MerchApi.Grpc;
// using static WeatherForecast.WeatherForecasts;

namespace OzonEdu.MerchApi.GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {

            using var channel = GrpcChannel.ForAddress("http://localhost:5001");
            var client = new MerchApiGrpc.MerchApiGrpcClient(channel);
            // var client = new WeatherForecastsClient(channel);
          
            
            try
            {
                var response = await client.RequestMerchAsync(new RequestMerchRequest
                    {EmployeeId = 111, MerchType = MerchType.VeteranPack});
                // var reply = await client.GetWeatherAsync(new Empty());
            }
            catch (RpcException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}