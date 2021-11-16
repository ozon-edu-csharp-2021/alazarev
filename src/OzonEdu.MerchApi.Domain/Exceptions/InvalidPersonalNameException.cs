using System;

namespace OzonEdu.MerchApi.Domain.Exceptions
{
    public class InvalidPersonalNameException : DomainException
    {
        public InvalidPersonalNameException(string message) : base(message)
        {
        }

        public InvalidPersonalNameException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}