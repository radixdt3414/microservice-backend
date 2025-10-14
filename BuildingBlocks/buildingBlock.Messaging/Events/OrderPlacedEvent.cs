using MassTransit;

namespace buildingBlock.Messaging.Events
{
    public record OrderPlacedEvent : IntegrationEvent, CorrelatedBy<Guid>
    {
        public Guid OrderId { get; set; } = default!;
        public Guid CustomerId { get; set; } = default!;

        //Shipping address
        public string Shipping_Country { get; set; } = default!;
        public string Shipping_Landmark { get; set; } = default!;
        public string Shipping_State { get; set; } = default!;
        public string Shipping_City { get; set; } = default!;
        public string Shipping_PostalCode { get; set; } = default!;
        public string Shipping_Description { get; set; } = default!;

        public List<BaskteItem> items { get; set; } = new List<BaskteItem>();

    }
}