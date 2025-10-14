namespace inventory.Domain.DTOs
{
    public class WarehouseDTO
    {
        public List<InventoryItemDTO> InventoryItem { get; set; } = new List<InventoryItemDTO>();
        public Guid Id { get; set; } = default(Guid);
        public string Landmark { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string State { get; set; } = default!;
        public string City { get; set; } = default!;
        public string PostalCode { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal? Latitude { get;  set; } = default!;
        public decimal? Longitude { get;  set; } = default!;
        public bool IsActive { get; set; } = default!;
        public int CurrentCapacityUnitUsed { get; set; } = default!;
        public int CapacityUnit { get; set; } = default!;
    }
}
