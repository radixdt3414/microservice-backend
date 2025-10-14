using order.Domain.Models;

namespace order.Domain.Events
{
    public record OrderItemUpdateEvent(Order Order) : IDomainEvent
    {
    }
}
