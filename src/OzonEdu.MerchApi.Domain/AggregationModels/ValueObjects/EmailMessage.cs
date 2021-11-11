using System.Collections.Generic;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects
{
    public class EmailMessage : ValueObject
    {
        public string Body { get; }
        public Email EmailAddress { get; }

        public EmailMessage(Email emailAddress, string body)
        {
            Body = body;
            EmailAddress = emailAddress;
            throw new System.NotImplementedException();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EmailAddress;
            yield return Body;
        }
    }
}