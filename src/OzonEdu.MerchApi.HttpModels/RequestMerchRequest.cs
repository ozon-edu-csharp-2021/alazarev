using CSharpCourse.Core.Lib.Enums;

namespace OzonEdu.MerchApi.HttpModels
{
    public sealed record RequestMerchRequest(int EmployeeId, MerchType MerchType);
}