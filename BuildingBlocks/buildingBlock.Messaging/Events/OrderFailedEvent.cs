namespace buildingBlock.Messaging.Events
{
    public record OrderFailedEvent : IntegrationEvent
    {
        public string UserName { get; set; } = string.Empty;
        public List<OrderFailedCartItem> Items { get; set; } = new List<OrderFailedCartItem>();
        public string reason { get; set; } = default!;
    }

    public class OrderFailedCartItem
    {
        public long Quentity { get; set; }
        public Guid ProductId { get; set; }
    }
}