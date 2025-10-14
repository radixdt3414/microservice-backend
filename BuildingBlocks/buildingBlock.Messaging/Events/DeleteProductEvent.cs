namespace buildingBlock.Messaging.Events
{
    public record DeleteProductEvent : IntegrationEvent
    {
        public Guid Id { get; set; } = Guid.Empty;
    }
    
}
