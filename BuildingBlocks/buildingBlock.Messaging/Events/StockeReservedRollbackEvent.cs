using MassTransit;

namespace buildingBlock.Messaging.Events
{
    public record StockeReservedRollbackEvent : IntegrationEvent, CorrelatedBy<Guid>
    {
        public List<OrderFailedCartItem> Items { get; set; } = new List<OrderFailedCartItem>();
        public Guid orderId { get; set; } = Guid.Empty;
        public string reason { get; set; } = default!;
    }
}