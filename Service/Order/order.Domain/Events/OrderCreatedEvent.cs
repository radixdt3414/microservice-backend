using order.Domain.Models;

namespace order.Domain.Events
{
    public record OrderCreatedEvent(Order Order) : IDomainEvent { }
}