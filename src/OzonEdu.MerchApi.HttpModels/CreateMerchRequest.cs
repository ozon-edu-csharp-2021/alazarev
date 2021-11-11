using CSharpCourse.Core.Lib.Enums;

namespace OzonEdu.MerchApi.HttpModels
{
    public sealed record CreateMerchRequest(string EmployeeEmail, MerchType MerchType);
}