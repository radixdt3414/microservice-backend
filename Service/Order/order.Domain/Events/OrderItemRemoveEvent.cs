using order.Domain.Models;

namespace order.Domain.Events
{
    public record OrderItemRemoveEvent(Order Order) : IDomainEvent
    {
    }
}
