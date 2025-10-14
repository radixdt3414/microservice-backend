namespace buildingBlock.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(Guid id, string entity) : base($"{entity} with {id} not found.") { }

        public NotFoundException(string user, string entity) : base($"{entity} with {user} not found.") { }

        public NotFoundException(string msg) : base(msg) { }
    }
}
