using System;
using System.Collections.Generic;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate
{
    public class EmailMessage : ValueObject
    {
        public string Body { get; }
        public Email EmailAddress { get; }

        private EmailMessage(Email emailAddress, string body)
        {
            Body = body ?? throw new ArgumentNullException(nameof(body));
            EmailAddress = emailAddress ?? throw new ArgumentNullException(nameof(emailAddress));
        }

        public static EmailMessage Create(Email emailAddress, string body) => new(emailAddress, body);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EmailAddress;
            yield return Body;
        }
    }
}