
using Microsoft.Extensions.Logging;

namespace order.Application.Order.Events.DomainEvent
{
    public class OrderUpdatedEventHandler(ILogger<OrderCreatedEventHandler> logger) : INotificationHandler<OrderUpdatedEvent>
    {
        public async Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation($"OrderUpdatedEvent Handled successfully! Order data: {notification.Order}");
            await Task.CompletedTask;
        }
    }
}
