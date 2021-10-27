using System.Dynamic;
using CSharpCourse.Core.Lib.Enums;

namespace OzonEdu.MerchApi.HttpModels
{
    public sealed record RequestMerchViewModel(int EmployeeId, MerchType MerchType);
}