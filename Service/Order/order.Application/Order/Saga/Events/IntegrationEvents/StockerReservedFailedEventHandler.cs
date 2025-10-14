using buildingBlock.Messaging.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using order.Application.Exceptions;
using order.Application.Order.Commands.OrderDelete;
using order.Application.Order.Commands.OrderFailed;
using order.Domain.ValueObjects;

namespace order.Application.Order.Saga.Events.IntegrationEvents
{
    public class StockeReservedFailedEventHandler(ISender sender, ILogger<StockeReservedFailedEventHandler> logger) : IConsumer<StockeReserveFailedEvent>
    {
        public async Task Consume(ConsumeContext<StockeReserveFailedEvent> context)
        {
            logger.LogInformation("StockeReservedFailedEventHandler: StockeReserveFailedEvent invoked.");
            var orderId = context.Message.OrderId;
            logger.LogInformation("StockeReservedFailedEventHandler: OrderFailedCommand dispatch.");
            var result = await sender.Send(new OrderFailedCommand
            {
                Id = OrderId.of(orderId)
            });
            if (!result.IsSuccess)
            {
                logger.LogError("StockeReservedFailedEventHandler: While rollback order place transaction encounter error.");
                //throw new OrderModificationFailedException();
            }
        }
    }
}