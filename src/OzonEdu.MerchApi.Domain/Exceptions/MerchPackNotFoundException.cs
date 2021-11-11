using System;

namespace OzonEdu.MerchApi.Domain.Exceptions
{
    public class MerchPackNotFoundException : DomainException
    {
        private const string _message = "Merch pack not found";

        public MerchPackNotFoundException() : base(_message)
        {
        }

        public MerchPackNotFoundException(Exception innerException) : base(_message, innerException)
        {
        }
    }
}