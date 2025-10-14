namespace inventory.Domain.DTOs
{
    public class InventoryItemDTO
    {
        public Guid Id { get; set; } = default(Guid);
        public Guid ProductId { get; set; } = default!;
        public Guid WarehouseId {get; set; } = default!;
        public int QuantityOnHand {get; set; } = default!;
        public int QuantityReserved {get; set; } = default!;
        public int QuantityAvailable { get; set; } = default!;
    }
}
