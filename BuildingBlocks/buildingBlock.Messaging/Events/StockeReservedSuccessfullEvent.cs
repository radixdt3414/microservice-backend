using MassTransit;

namespace buildingBlock.Messaging.Events
{
    public record StockeReservedSuccessfullEvent() : IntegrationEvent, CorrelatedBy<Guid> { 
        public Guid orderId { get; set; }
    }   
}