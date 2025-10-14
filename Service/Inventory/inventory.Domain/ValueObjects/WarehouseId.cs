using inventory.Domain.Exceptions;

namespace inventory.Domain.ValueObjects
{
    public record WarehouseId 
    {
        public Guid Value { get; set; } = default!;
        private WarehouseId(Guid val) => Value = val;
        public static WarehouseId Of(Guid id)
        {
            ArgumentNullException.ThrowIfNull(id);
            if (id == Guid.Empty)
            {
                throw new DomainNullException("Warehouse id couldn't be null or empty");
            }

            return new WarehouseId(id);
        }
    }
}