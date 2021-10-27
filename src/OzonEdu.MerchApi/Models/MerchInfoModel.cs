using System;
using CSharpCourse.Core.Lib.Enums;

namespace OzonEdu.MerchApi.Models
{
    /// <summary>
    /// Информация по выданному мерчу
    /// </summary>
    /// <param name="MerchType">Тип мерча</param>
    /// <param name="ReceivingDate">Дала выдачи</param>
    public record MerchInfoModel(MerchType MerchType, DateTime ReceivingDate);
}