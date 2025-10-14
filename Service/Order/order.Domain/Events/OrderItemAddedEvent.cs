using order.Domain.Models;

namespace order.Domain.Events
{
    public record OrderItemAddedEvent(Order Order) : IDomainEvent
    {
    }
}
