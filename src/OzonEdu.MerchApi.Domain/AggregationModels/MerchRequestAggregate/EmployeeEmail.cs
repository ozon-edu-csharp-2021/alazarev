namespace OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects
{
    public class EmployeeEmail : Email
    {
        protected EmployeeEmail(string email) : base(email)
        {
        }

        public static EmployeeEmail Create(string email) => new(email);
    }
}