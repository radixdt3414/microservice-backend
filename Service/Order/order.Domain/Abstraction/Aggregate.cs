namespace order.Domain.Abstraction
{
    public abstract class Aggregate<TId> : Entity<TId>, IAggregate<TId>
    {
        private List<IDomainEvent> _DomainEvents = new List<IDomainEvent>();
        public IReadOnlyList<IDomainEvent> DomainEvents => _DomainEvents.AsReadOnly();

        public void AddDomainEvents(IDomainEvent domainEvent)
        {
            _DomainEvents.Add(domainEvent);
        }

        public List<IDomainEvent> ClearDomainEvents()
        {
            var events =  _DomainEvents;
            _DomainEvents.Clear();
            return events;
        }
    }
}