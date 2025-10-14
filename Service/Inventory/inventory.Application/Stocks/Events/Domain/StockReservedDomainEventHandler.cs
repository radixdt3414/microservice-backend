using buildingBlock.Messaging.Events;
using inventory.Application.Data;
using inventory.Domain.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace inventory.Application.Stocks.Events.Domain
{
    public class StockReservedDomainEventHandler(IPublishEndpoint publisher, ICorrelationContext correlationContext, ILogger<StockReservedDomainEventHandler> logger) : INotificationHandler<StockReservedDomainEvent>
    {
        public async Task Handle(StockReservedDomainEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation($"--------------------inventory------> StockReservedDomainEventHandler: StockReservedDomainEventHandler invoked.");
            logger.LogInformation($"--------------------inventory------> StockReservedDomainEventHandler: StockeReservedSuccessfullEvent dispatch.");
            await publisher.Publish(new StockeReservedSuccessfullEvent
            {
                CorrelationId = correlationContext.CorrelationId,
                orderId = notification.orderId
            });
        }
    }
}
