namespace OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects
{
    public class ManagerEmail : Email
    {
        protected ManagerEmail(string email) : base(email)
        {
        }

        public static ManagerEmail Create(string email) => new(email);
    }
}