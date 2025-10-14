namespace buildingBlock.Messaging.Events
{
    public record StockeReservedEvent : IntegrationEvent {
        public Guid orderId { get; set; }

    };
    
}
