using buildingBlock.Messaging.Events;
using inventory.Application.Stocks.Commands.RemoveInventory;
using inventory.Application.Stocks.Events.Saga.IntegrationEvents;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace inventory.Application.Stocks.Events.IntegrationEvent
{
    public class DeleteProductEventConsumer(ISender sender, ILogger<OrderPlacedHandler> logger) : IConsumer<DeleteProductEvent>
    {
        public async Task Consume(ConsumeContext<DeleteProductEvent> context)
        {
            logger.LogInformation($"--------------------inventory------> DeleteProductEventConsumer: DeleteProductEventConsumer invoked.");
            var command = new RemoveInventoryCommand(context.Message.Id);
            var response = await sender.Send(command);
            logger.LogInformation($"--------------------inventory------> DeleteProductEventConsumer: Removed inventory successfully.");
        }
    }
}
