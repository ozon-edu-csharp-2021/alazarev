using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using RequestMerchRequestGrpc = OzonEdu.MerchApi.Grpc.RequestMerchRequest;
using RequestMerchResponseGrpc = OzonEdu.MerchApi.Grpc.RequestMerchResponse;
using GetReceivingMerchInfoRequestGprc = OzonEdu.MerchApi.Grpc.GetReceivingMerchInfoRequest;
using GetReceivingMerchInfoResponseGprc = OzonEdu.MerchApi.Grpc.GetReceivingMerchInfoResponse;
using RequestMerchRequestHttp = OzonEdu.MerchApi.HttpModels.RequestMerchRequest;
using RequestMerchResponseHttp = OzonEdu.MerchApi.HttpModels.RequestMerchResponse;
using GetReceivingMerchInfoRequestHttp = OzonEdu.MerchApi.HttpModels.GetReceivingMerchInfoRequest;
using GetReceivingMerchInfoResponseHttp = OzonEdu.MerchApi.HttpModels.GetReceivingMerchInfoResponse;
using MerchInfoHttp = OzonEdu.MerchApi.HttpModels.MerchInfo;
using MerchInfoGrpc = OzonEdu.MerchApi.Grpc.MerchInfo;

namespace OzonEdu.MerchApi.Mappers
{
    public sealed class GprcMappingProfile : Profile
    {
        public GprcMappingProfile()
        {
            CreateMap<RequestMerchRequestGrpc, RequestMerchRequestHttp>();
            CreateMap<GetReceivingMerchInfoRequestGprc, GetReceivingMerchInfoRequestHttp>();

            CreateMap<RequestMerchResponseHttp, RequestMerchResponseGrpc>();
            CreateMap<GetReceivingMerchInfoResponseHttp, GetReceivingMerchInfoResponseGprc>();

            CreateMap<MerchInfoHttp, MerchInfoGrpc>().ForMember(
                h => h.ReceivingDate,
                o => o.MapFrom(s => s.ReceivingDate.ToTimestamp()));
        }
    }
}