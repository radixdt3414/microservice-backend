namespace inventory.Application.Exceptions
{
    public class WarehouseNotFoundException: Exception
    {
        public WarehouseNotFoundException(Guid Id) : base($"Warehouse with Id{Id} not found.") { }
    }
}
