using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using OzonEdu.MerchApi.Grpc;
using OzonEdu.MerchApi.Mappers;
using OzonEdu.MerchApi.Services.Interfaces;
using RequestMerchModelStatus = OzonEdu.MerchApi.Models.RequestMerchStatus;

namespace OzonEdu.MerchApi.GrpcServices
{
    public sealed class MerchApiGrpcService : MerchApiGrpc.MerchApiGrpcBase
    {
        private readonly IMerchService _merchService;

        public MerchApiGrpcService(IMerchService merchService)
        {
            _merchService = merchService;
        }

        public override async Task<RequestMerchResponse> RequestMerch(RequestMerchRequest request,
            ServerCallContext context)
        {
            var merchTypeFromGrpc = GrpcMapper.MerchTypeToModelType(request.MerchType);

            var result =
                await _merchService.RequestMerchAsync(request.EmployeeId, merchTypeFromGrpc, context.CancellationToken);

            return new RequestMerchResponse
            {
                Status = GrpcMapper.RequestMerchStatusToGrpc(result)
            };
        }

        public override async Task<GetReceivingMerchInfoResponse> GetReceivingMerchInfo(
            GetReceivingMerchInfoRequest request,
            ServerCallContext context)
        {
            var models = await _merchService.GetReceivingMerchInfoAsync(request.EmployeeId, context.CancellationToken);

            return new GetReceivingMerchInfoResponse
            {
                Items =
                {
                    models.Select(GrpcMapper.MerchInfoModelToGrpc)
                }
            };
        }
    }
}