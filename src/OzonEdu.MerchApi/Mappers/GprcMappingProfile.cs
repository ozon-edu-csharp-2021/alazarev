using System;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.HttpModels;
using RequestMerchRequestGrpc = OzonEdu.MerchApi.Grpc.RequestMerchRequest;
using RequestMerchResponseGrpc = OzonEdu.MerchApi.Grpc.RequestMerchResponse;
using GetReceivingMerchInfoRequestGprc = OzonEdu.MerchApi.Grpc.GetMerchInfoRequest;
using GetReceivingMerchInfoResponseGprc = OzonEdu.MerchApi.Grpc.GetMerchInfoResponse;
using RequestMerchResponseHttp = OzonEdu.MerchApi.HttpModels.RequestMerchResponse;
using GetReceivingMerchInfoRequestHttp = OzonEdu.MerchApi.HttpModels.GetMerchInfoRequest;
using GetReceivingMerchInfoResponseHttp = OzonEdu.MerchApi.HttpModels.GetMerchInfoResponse;
using MerchInfoHttp = OzonEdu.MerchApi.HttpModels.MerchInfo;
using MerchInfoGrpc = OzonEdu.MerchApi.Grpc.MerchInfo;

namespace OzonEdu.MerchApi.Mappers
{
    public sealed class GprcMappingProfile : Profile
    {
        public GprcMappingProfile()
        {
            CreateMap<RequestMerchRequestGrpc, CreateMerchRequest>();

            CreateMap<GetReceivingMerchInfoRequestGprc, GetReceivingMerchInfoRequestHttp>();

            CreateMap<MerchRequestStatus, RequestMerchResponseGrpc>().ForMember(
                h => h.Status,
                o => o.MapFrom(s => FromMerchRequestStatus(s)));

            CreateMap<GetReceivingMerchInfoResponseHttp, GetReceivingMerchInfoResponseGprc>();

            CreateMap<MerchRequest, MerchInfoGrpc>().ForMember(
                h => h.ReceivingDate,
                o => o.MapFrom(s => s.ReservedAt.Value.ToTimestamp()));
        }

        public static Grpc.RequestMerchStatus FromMerchRequestStatus(MerchRequestStatus merchRequestStatus)
        {
            return merchRequestStatus.Id switch
            {
                1 => Grpc.RequestMerchStatus.Created,
                2 => Grpc.RequestMerchStatus.InProcess,
                3 => Grpc.RequestMerchStatus.WaitingForSupply,
                4 => Grpc.RequestMerchStatus.Reserved,
                5 => Grpc.RequestMerchStatus.Informed,
                6 => Grpc.RequestMerchStatus.Error,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}