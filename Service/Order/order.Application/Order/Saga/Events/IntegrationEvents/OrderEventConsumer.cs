using buildingBlock.Messaging.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using order.Application.Order.Commands.OrderCreate;
using System.Text.Json;

namespace order.Application.Order.Saga.Events.IntegrationEvents
{
    public class OrderEventConsumer(
        ILogger<OrderEventConsumer> logger, 
        ISender sender,
        ICorrelationContext correlationContext) : IConsumer<OrderEvent>
    {
        public async Task Consume(ConsumeContext<OrderEvent> context)
        {
            var checkoutDetails = context.Message;
            logger.LogInformation("--------------------order------> Integration Event: Order event handler invoked");
            logger.LogInformation($"--------------------order------> Data: {JsonSerializer.Serialize(context.Message)}");
            correlationContext.CorrelationId = context.Message.CorrelationId;
            var result = await sender.Send(new OrderCreateCommand(Converter.EventToDTOConverter(checkoutDetails)));
        }
    }
}
