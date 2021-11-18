using System;

namespace OzonEdu.MerchApi.Domain.Exceptions
{
    public class ManagerEmailIsNullException : DomainException
    {
        private const string _message = "Manager email is null";

        public ManagerEmailIsNullException() : base(_message)
        {
        }

        public ManagerEmailIsNullException(Exception innerException) : base(_message, innerException)
        {
        }
        
    }
}