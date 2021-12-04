using CSharpCourse.Core.Lib.Enums;

namespace OzonEdu.MerchApi.HttpModels
{
    public sealed record CreateMerchPackRequest(MerchType MerchType, MerchPackItemModel[] Items);

    public sealed record MerchPackItemModel(long ItemId, int Quantity);
}