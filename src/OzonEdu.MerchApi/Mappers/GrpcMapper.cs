using System;
using Google.Protobuf.WellKnownTypes;
using OzonEdu.MerchApi.Grpc;
using OzonEdu.MerchApi.GrpcServices;
using OzonEdu.MerchApi.Models;
using RequestMerchModelStatus = OzonEdu.MerchApi.Models.RequestMerchStatus;
using RequestMerchStatus = OzonEdu.MerchApi.Grpc.RequestMerchStatus;

namespace OzonEdu.MerchApi.Mappers
{
    /// <summary>
    /// Маппер для работы с <see cref="MerchApiGrpcService"/>
    /// </summary>
    public static class GrpcMapper
    {
        public static Func<MerchInfoModel, MerchInfo> MerchInfoModelToGrpc => m => new MerchInfo
        {
            MerchType = MerchTypeToGrpc(m.MerchType),
            ReceivingDate = m.ReceivingDate.ToTimestamp()
        };

        public static Func<CSharpCourse.Core.Lib.Enums.MerchType, MerchType> MerchTypeToGrpc => t => t switch
        {
            CSharpCourse.Core.Lib.Enums.MerchType.VeteranPack => MerchType.VeteranPack,
            CSharpCourse.Core.Lib.Enums.MerchType.WelcomePack => MerchType.WelcomePack,
            CSharpCourse.Core.Lib.Enums.MerchType.ConferenceListenerPack => MerchType
                .ConferenceListenerPack,
            CSharpCourse.Core.Lib.Enums.MerchType.ConferenceSpeakerPack => MerchType
                .ConferenceSpeakerPack,
            CSharpCourse.Core.Lib.Enums.MerchType.ProbationPeriodEndingPack => MerchType
                .ProbationPeriodEndingPack,
            _ => throw new ArgumentOutOfRangeException()
        };

        public static Func<RequestMerchModelStatus, RequestMerchStatus> RequestMerchStatusToGrpc => s => s switch
        {
            RequestMerchModelStatus.Reserved => RequestMerchStatus.Reserved,
            RequestMerchModelStatus.AlreadyReceived => RequestMerchStatus.AlreadyReceived,
            RequestMerchModelStatus.NotAvailable => RequestMerchStatus.NotAvailable,
            _ => throw new ArgumentOutOfRangeException()
        };

        public static Func<MerchType, CSharpCourse.Core.Lib.Enums.MerchType> MerchTypeToModelType => t => t switch
        {
            MerchType.VeteranPack => CSharpCourse.Core.Lib.Enums.MerchType.VeteranPack,
            MerchType.WelcomePack => CSharpCourse.Core.Lib.Enums.MerchType.WelcomePack,
            MerchType.ConferenceListenerPack => CSharpCourse.Core.Lib.Enums.MerchType.ConferenceListenerPack,
            MerchType.ConferenceSpeakerPack => CSharpCourse.Core.Lib.Enums.MerchType.ConferenceSpeakerPack,
            MerchType.ProbationPeriodEndingPack => CSharpCourse.Core.Lib.Enums.MerchType.ProbationPeriodEndingPack,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}