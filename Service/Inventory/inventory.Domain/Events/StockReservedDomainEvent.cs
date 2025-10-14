namespace inventory.Domain.Events
{
    public record StockReservedDomainEvent(Guid orderId) : IDomainEvent;   
}