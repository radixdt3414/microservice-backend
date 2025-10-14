using MassTransit;

namespace buildingBlock.Messaging.Events
{
    public record OrderPlacedRollbackEvent : IntegrationEvent, CorrelatedBy<Guid>
    {
        public List<OrderFailedCartItem> Items { get; set; } = new List<OrderFailedCartItem>();
        public string reason { get; set; } = default!;
    }
}
