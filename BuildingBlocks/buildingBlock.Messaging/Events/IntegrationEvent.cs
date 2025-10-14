using MassTransit;

namespace buildingBlock.Messaging.Events
{
    public record IntegrationEvent 
    {
        public Guid CorrelationId { get; set; }
        public Guid EventId => Guid.NewGuid();
        public DateTime OccuredOn => DateTime.UtcNow;
        public string EventType => GetType().AssemblyQualifiedName;
    }
}