namespace inventory.Domain.Abstraction
{
    public abstract class Aggregate<T> : Entity<T>, IAggregate
    {
        private List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public List<IDomainEvent> CleardomainEvent()
        {
            var temp = _domainEvents;
            _domainEvents.Clear();
            return temp;
        }
    }

    public interface IAggregate : IEntity
    {
        private List<IDomainEvent> _domainEvents => new List<IDomainEvent>();
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent domainEvent);

        public List<IDomainEvent> CleardomainEvent();
    }
}
