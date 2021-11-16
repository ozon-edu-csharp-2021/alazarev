using System;

namespace OzonEdu.MerchApi.Domain.Exceptions
{
    public class ManagerNotFoundException : DomainException
    {
        private const string _message = "HR-manager not found";

        public ManagerNotFoundException() : base(_message)
        {
        }

        public ManagerNotFoundException(Exception innerException) : base(_message, innerException)
        {
        }
    }
}