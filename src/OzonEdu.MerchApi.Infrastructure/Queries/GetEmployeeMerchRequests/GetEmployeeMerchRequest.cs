using System;
using CSharpCourse.Core.Lib.Enums;

namespace OzonEdu.MerchApi.Infrastructure.Queries.GetEmployeeMerchRequests
{
    public class GetEmployeeMerchRequest
    {
        public string Status { get; set; }
        public int RequestId { get; set; }
        public MerchType MerchType { get; set; }

        public DateTimeOffset StartedAt { get; set; }
        public DateTimeOffset? ReservedAt { get; set; }
    }
}