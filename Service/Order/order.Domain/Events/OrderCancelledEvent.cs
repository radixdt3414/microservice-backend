using order.Domain.Models;

namespace order.Domain.Events
{
    public record OrderCancelledEvent(Order Order) : IDomainEvent
    {
    }
}
