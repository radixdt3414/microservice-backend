namespace buildingBlock.Messaging.Events
{
    public record StockeReserveFailedEvent: IntegrationEvent
    {
        public Guid OrderId { get; set; }
    }
}