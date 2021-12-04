using CSharpCourse.Core.Lib.Enums;

namespace OzonEdu.MerchApi.Domain.Contracts.EmployeesService
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string BirthDay { get; set; }
        public string HiringDate { get; set; }
        public string Email { get; set; }
        public ClothingSize ClothingSize { get; set; }
    }
}