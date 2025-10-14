namespace buildingBlock.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string property, string message) : base($"{property}: {message}") { }
        public BadRequestException(Exception ex) : base("Bad request",ex) { }
    }
}
