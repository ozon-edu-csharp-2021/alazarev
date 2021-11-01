using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using OzonEdu.MerchApi.Grpc;
using OzonEdu.MerchApi.Services.Interfaces;
using RequestMerchRequestGrpc = OzonEdu.MerchApi.Grpc.RequestMerchRequest;
using RequestMerchResponseGrpc = OzonEdu.MerchApi.Grpc.RequestMerchResponse;
using GetReceivingMerchInfoRequestGprc = OzonEdu.MerchApi.Grpc.GetReceivingMerchInfoRequest;
using GetReceivingMerchInfoResponseGprc = OzonEdu.MerchApi.Grpc.GetReceivingMerchInfoResponse;
using RequestMerchRequestHttp = OzonEdu.MerchApi.HttpModels.RequestMerchRequest;
using RequestMerchResponseHttp = OzonEdu.MerchApi.HttpModels.RequestMerchResponse;
using GetReceivingMerchInfoRequestHttp = OzonEdu.MerchApi.HttpModels.GetReceivingMerchInfoRequest;
using GetReceivingMerchInfoResponseHttp = OzonEdu.MerchApi.HttpModels.GetReceivingMerchInfoResponse;

namespace OzonEdu.MerchApi.GrpcServices
{
    public sealed class MerchApiGrpcService : MerchApiGrpc.MerchApiGrpcBase
    {
        private readonly IMerchService _merchService;
        private readonly IMapper _mapper;

        public MerchApiGrpcService(IMerchService merchService, IMapper mapper)
        {
            _merchService = merchService;
            _mapper = mapper;
        }

        public override async Task<RequestMerchResponse> RequestMerch(RequestMerchRequest request,
            ServerCallContext context)
        {
            
            var result =
                await _merchService.RequestMerchAsync(_mapper.Map<RequestMerchRequestHttp>(request) ,
                    context.CancellationToken);

            return _mapper.Map<RequestMerchResponseGrpc>(result);
        }

        public override async Task<GetReceivingMerchInfoResponse> GetReceivingMerchInfo(
            GetReceivingMerchInfoRequest request,
            ServerCallContext context)
        {
            var result =
                await _merchService.GetReceivingMerchInfoAsync(_mapper.Map<GetReceivingMerchInfoRequestHttp>(request),
                    context.CancellationToken);

            return _mapper.Map<GetReceivingMerchInfoResponseGprc>(result);
        }
    }
}