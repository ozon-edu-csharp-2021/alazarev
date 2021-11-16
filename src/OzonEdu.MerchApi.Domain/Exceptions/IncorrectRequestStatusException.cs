using System;

namespace OzonEdu.MerchApi.Domain.Exceptions
{
    public class IncorrectRequestStatusException : DomainException
    {
        private const string _message = "Incorrect request status";

        public IncorrectRequestStatusException() : base(_message)
        {
        }

        public IncorrectRequestStatusException(Exception innerException) : base(_message, innerException)
        {
        }
    }
}