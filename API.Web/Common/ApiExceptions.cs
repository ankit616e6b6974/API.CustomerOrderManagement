namespace API.Web.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entity, object key)
            : base($"{entity} with id '{key}' was not found.") { }
    }

    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message) : base(message) { }
    }
}
