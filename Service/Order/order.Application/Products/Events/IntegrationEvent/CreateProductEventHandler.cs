using buildingBlock.Messaging.Events;
using Mapster;
using MassTransit;
using Microsoft.Extensions.Logging;
using order.Application.Order.Saga.Events.IntegrationEvents;
using order.Application.Products.Commands.CreateProduct;

namespace order.Application.Products.Events.IntegrationEvent
{
    public class CreateProductEventHandler(ISender sender, ILogger<StockeReservedEventHandler> logger) : IConsumer<CreateProductEvent>
    {
        public async Task Consume(ConsumeContext<CreateProductEvent> context)
        {
            logger.LogInformation($"--------------------order------> CreateProductEventHandler: CreateProductEventHandler invoked.");
            var command = context.Message.Adapt<CreateProductCommand>();
            var result = await sender.Send(command);
        }
    }
}
