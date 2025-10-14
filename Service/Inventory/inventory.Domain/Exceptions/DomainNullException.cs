namespace inventory.Domain.Exceptions
{
    public class DomainNullException : Exception
    {
        public DomainNullException(string msg) : base($"Domain exception: {msg}") { }
    }
}
