namespace buildingBlock.Messaging.Events
{
    public record CreateProductEvent : IntegrationEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; } = default!;
    }
}
