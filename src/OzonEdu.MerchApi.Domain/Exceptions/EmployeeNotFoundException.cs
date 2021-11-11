using System;

namespace OzonEdu.MerchApi.Domain.Exceptions
{
    public class EmployeeNotFoundException : DomainException
    {
        private const string _message = "Employee not found";
        private const string _messageTemplate = "Employee {0} not found";

        public EmployeeNotFoundException() : base(_message)
        {
        }

        public EmployeeNotFoundException(Exception innerException) : base(_message, innerException)
        {
        }

        public EmployeeNotFoundException(string email) : base(string.Format(_messageTemplate, email))
        {
        }

        public EmployeeNotFoundException(string email, Exception innerException) : base(string.Format(_messageTemplate,
            email), innerException)
        {
        }
    }
}