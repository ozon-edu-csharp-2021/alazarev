using System;
using CSharpCourse.Core.Lib.Enums;

namespace OzonEdu.MerchApi.HttpModels
{
    public sealed record MerchInfoViewModel(MerchType MerchType, DateTime ReceivingDate);
}