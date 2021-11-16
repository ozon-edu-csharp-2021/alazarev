using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.Contracts.DomainServices.MerchRequestService;
using OzonEdu.MerchApi.Grpc;
using OzonEdu.MerchApi.HttpModels;
using RequestMerchRequestGrpc = OzonEdu.MerchApi.Grpc.RequestMerchRequest;
using RequestMerchResponseGrpc = OzonEdu.MerchApi.Grpc.RequestMerchResponse;
using GetReceivingMerchInfoRequestGprc = OzonEdu.MerchApi.Grpc.GetMerchInfoRequest;
using GetReceivingMerchInfoResponseGprc = OzonEdu.MerchApi.Grpc.GetMerchInfoResponse;
using RequestMerchResponseHttp = OzonEdu.MerchApi.HttpModels.RequestMerchResponse;
using GetReceivingMerchInfoRequestHttp = OzonEdu.MerchApi.HttpModels.GetMerchInfoRequest;
using GetReceivingMerchInfoResponseHttp = OzonEdu.MerchApi.HttpModels.GetMerchInfoResponse;

namespace OzonEdu.MerchApi.GrpcServices
{
    public sealed class MerchApiGrpcService : MerchApiGrpc.MerchApiGrpcBase
    {
        private readonly IMerchRequestService _merchService;
        private readonly IMapper _mapper;

        public MerchApiGrpcService(IMerchRequestService merchService, IMapper mapper)
        {
            _merchService = merchService;
            _mapper = mapper;
        }

        public override async Task<RequestMerchResponseGrpc> RequestMerch(RequestMerchRequest request,
            ServerCallContext context)
        {
            var mappedRequest = _mapper.Map<CreateMerchRequest>(request);

            var result =
                await _merchService.CreateMerchRequestAsync(Email.Create(mappedRequest.EmployeeEmail),
                    mappedRequest.MerchType,
                    MerchRequestMode.ByRequest,
                    context.CancellationToken);

            return _mapper.Map<RequestMerchResponseGrpc>(result.Status);
        }

        public override async Task<GetReceivingMerchInfoResponseGprc> GetReceivingMerchInfo(
            GetReceivingMerchInfoRequestGprc request,
            ServerCallContext context)
        {
            var result =
                await _merchService.GetMerchInfoAsync(Email.Create(request.EmployeeEmail),
                    context.CancellationToken);

            return _mapper.Map<GetReceivingMerchInfoResponseGprc>(result);
        }
    }
}