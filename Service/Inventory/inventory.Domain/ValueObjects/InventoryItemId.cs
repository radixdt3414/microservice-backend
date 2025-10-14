using inventory.Domain.Exceptions;

namespace inventory.Domain.ValueObjects
{
    public record InventoryItemId
    {
        public Guid Value { get; set; } = default!;
        private InventoryItemId(Guid val) => Value = val;
        public static InventoryItemId Of(Guid id)
        {
            ArgumentNullException.ThrowIfNull(id);
            if(id == Guid.Empty)
            {
                throw new DomainNullException("Inventory item id couldn't be null or empty");
            }

            return new InventoryItemId(id);
        }
    }
}