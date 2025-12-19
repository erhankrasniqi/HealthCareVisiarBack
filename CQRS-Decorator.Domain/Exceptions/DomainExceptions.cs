namespace CQRS_Decorator.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
        
        public DomainException(string message, Exception innerException) 
            : base(message, innerException) { }
    }

    public class NotFoundException : DomainException
    {
        public NotFoundException(string entityName, object key)
            : base($"{entityName} with key '{key}' was not found.") { }
    }

    public class ValidationException : DomainException
    {
        public ValidationException(string message) : base(message) { }
    }

    public class BusinessRuleViolationException : DomainException
    {
        public BusinessRuleViolationException(string message) : base(message) { }
    }
}
