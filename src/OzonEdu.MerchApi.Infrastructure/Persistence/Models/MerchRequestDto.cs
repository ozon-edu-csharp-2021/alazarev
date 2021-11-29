using System;
using CSharpCourse.Core.Lib.Enums;

namespace OzonEdu.MerchApi.Infrastructure.Persistence.Models
{
    public class MerchRequestDto
    {
        public int Id { get; set; }
        public DateTimeOffset StartedAt { get; set; }
        public string ManagerEmail { get; set; }

        public string EmployeeEmail { get; set; }
        public int Status { get; set; }
        public int ClothingSize { get; set; }
        public int RequestedMerchType { get; set; }
        public int Mode { get; set; }
        public DateTimeOffset? ReservedAt { get; set; }
        
        public string RequestMerch { get; set; }
    }
}