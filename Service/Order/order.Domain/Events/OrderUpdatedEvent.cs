using order.Domain.Models;

namespace order.Domain.Events
{
    public record OrderUpdatedEvent(Order Order) : IDomainEvent
    {
    }
}
