namespace order.Domain.Abstraction
{
    public interface IAggregate<T> : IAggregate, IEntity<T>
    {

    }
    public interface IAggregate : IEntity
    {
        IReadOnlyList<IDomainEvent> DomainEvents { get; }
        public void AddDomainEvents(IDomainEvent domainEvent);
        public List<IDomainEvent> ClearDomainEvents();

    }
}
