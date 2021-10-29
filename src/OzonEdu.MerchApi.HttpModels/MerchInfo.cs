using System;
using CSharpCourse.Core.Lib.Enums;

namespace OzonEdu.MerchApi.HttpModels
{
    /// <summary>
    /// Информация по выданному мерчу
    /// </summary>
    /// <param name="MerchType">Тип мерча</param>
    /// <param name="ReceivingDate">Дала выдачи</param>
    public record MerchInfo(MerchType MerchType, DateTime ReceivingDate);
}