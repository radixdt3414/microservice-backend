using buildingBlock.Messaging.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using order.Application.Exceptions;
using order.Application.Order.Commands.OrderedSuccessful;
using order.Domain.ValueObjects;

namespace order.Application.Order.Saga.Events.IntegrationEvents
{
    public class StockeReservedEventHandler(ISender sender, ILogger<StockeReservedEventHandler> logger) : IConsumer<StockeReservedEvent>
    {
        public async Task Consume(ConsumeContext<StockeReservedEvent> context)
        {
            logger.LogInformation($"--------------------order------> StockeReservedEventHandler: StockeReservedEvent invoked.");
            var orderId = context.Message.orderId;
            logger.LogInformation($"--------------------order------> StockeReservedEventHandler: OrderedSuccessfulCommand dispatch.");
            var result = await sender.Send(new OrderedSuccessfulCommand(OrderId.of(orderId)));
            if (!result.IsSuccess)
            {
                throw new OrderModificationFailedException();
            }
        }
    }
}