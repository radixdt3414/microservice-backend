using buildingBlock.Messaging.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using System.Text.Json;

namespace order.Application.Order.Events.DomainEvent
{
    public class OrderCreatedEventHandler(
        ILogger<OrderCreatedEventHandler> logger, 
        IPublishEndpoint publisher, 
        IFeatureManager featureManager,
        ICorrelationContext correlationContext) : INotificationHandler<OrderCreatedEvent>
    {
        public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("--------------------order------> OrderCreatedEventHandler : OrderCreatedEventHandler invoked.");
            logger.LogInformation($"OrderCreatedEvent Handled successfully! Order data: {notification.Order}");
            if(await featureManager.IsEnabledAsync("Integration_Event"))
            {
                var orderArr = Converter.ModelToDTOConverter(new List<OrderModel>() { notification.Order }).ToArray();
                var orderPlacedMsg = Converter.DTOToEventConverter(orderArr.First());
                orderPlacedMsg.CorrelationId = correlationContext.CorrelationId;
                logger.LogInformation("--------------------order------> OrderCreatedEventHandler : OrderPlacedSuccessfullEvent dispatch.");
                var data = new OrderPlacedSuccessfullEvent
                {
                    OrderId = orderPlacedMsg.OrderId,
                    CustomerId = orderPlacedMsg.CustomerId,
                    CorrelationId = correlationContext.CorrelationId,
                    Shipping_Country = orderPlacedMsg.Shipping_Country,
                    Shipping_Landmark = orderPlacedMsg.Shipping_Landmark,
                    Shipping_State = orderPlacedMsg.Shipping_State,
                    Shipping_City = orderPlacedMsg.Shipping_City,
                    Shipping_PostalCode = orderPlacedMsg.Shipping_PostalCode,
                    Shipping_Description = orderPlacedMsg.Shipping_Description,
                    items = orderPlacedMsg.items
                };
                await publisher.Publish(data);
                logger.LogInformation($"--------------------order------> OrderCreatedEventHandler: Published event type: {nameof(OrderPlacedSuccessfullEvent)} - {typeof(OrderPlacedSuccessfullEvent).AssemblyQualifiedName}");
                logger.LogInformation("--------------------order------> OrderCreatedEventHandler : OrderPlacedSuccessfullEvent Data that we have dispatched data:.");
                logger.LogInformation($"--------------------order------> OrderCreatedEventHandler : OrderPlacedSuccessfullEvent Data : {JsonSerializer.Serialize(data)}");
            }
        }
    }
}