using CSharpCourse.Core.Lib.Enums;

namespace OzonEdu.MerchApi.HttpModels
{
    public sealed record CreateMerchRequest(int EmployeeId, string ManagerEmail, MerchType MerchType);
}