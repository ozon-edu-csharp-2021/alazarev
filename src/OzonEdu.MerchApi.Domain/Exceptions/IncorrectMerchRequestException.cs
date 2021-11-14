using System;

namespace OzonEdu.MerchApi.Domain.Exceptions
{
    public class IncorrectMerchRequestException : DomainException
    {
        private const string _message = "Incorrect merch request";

        public IncorrectMerchRequestException() : base(_message)
        {
        }

        public IncorrectMerchRequestException(Exception innerException) : base(_message, innerException)
        {
        }
    }
}