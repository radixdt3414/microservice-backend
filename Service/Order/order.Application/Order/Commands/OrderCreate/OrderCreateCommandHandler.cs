using buildingBlock.Messaging.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using order.Domain.Enum;
using order.Domain.ValueObjects;

namespace order.Application.Order.Commands.OrderCreate
{
    public class OrderCreateCommandHandler(IApplicationDbContext DbContext, 
            ILogger<OrderCreateCommandHandler> logger, 
            IPublishEndpoint publisher,
            ICorrelationContext correlationContext) : ICommandHandler<OrderCreateCommand, OrderCreateResponse>
    {
        public async Task<OrderCreateResponse> Handle(OrderCreateCommand command, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("--------------------order------> OrderCreateCommandHandler : Order create hndler invoked.");
                OrderModel order = CreateNewOrder(command.Order);
                logger.LogInformation("--------------------order------> OrderCreateCommandHandler : new order created.");
                DbContext.Orders.Add(order);
                logger.LogInformation("--------------------order------> OrderCreateCommandHandler : new order Added.");
                await DbContext.SaveChangesAsync(cancellationToken);
                logger.LogInformation("--------------------order------> OrderCreateCommandHandler : Order saved.");
                return new OrderCreateResponse(order.Id.Value);

            }
            catch(Exception ex)
            {
                logger.LogInformation("OrderCreateCommandHandler: While creating order encounter error.");
                if(correlationContext.CorrelationId != Guid.Empty)
                {
                    logger.LogInformation($"--------------------order------> OrderCreateCommandHandler Exception: will dispatch rollback event with correlation id{correlationContext.CorrelationId }");
                    var orderFailedEvent = new OrderPlacedRollbackEvent()
                    {
                        Items = command.Order.OrderItems.Select(x => new OrderFailedCartItem()
                        {
                            Quentity = x.Quentity,
                            ProductId = x.ProductId,
                        }).ToList(),
                        reason = "While creating order encounter error.",
                        CorrelationId = correlationContext.CorrelationId
                    };
                    logger.LogInformation($"--------------------order------> OrderCreateCommandHandler Exception: dispatch orderFailedEvent rollback event with Data {orderFailedEvent}");
                    await publisher.Publish(orderFailedEvent);
                }
                //throw new Exception("OrderCreateCommandHandler: While creating order encounter error.");
                return new OrderCreateResponse(Guid.Empty);
            }
        }

        public OrderModel CreateNewOrder(OrderDTO order)
        {
            var ShippingAddress = Address.of(
                order.ShippingAddressDetails.FirstName,
                order.ShippingAddressDetails.LastName,
                order.ShippingAddressDetails.Country,
                order.ShippingAddressDetails.Landmark,
                order.ShippingAddressDetails.State,
                order.ShippingAddressDetails.City,
                order.ShippingAddressDetails.PostalCode,
                order.ShippingAddressDetails.Description
                );

            var OrderAddress = Address.of(
                order.OrderAddressDetails.FirstName,
                order.OrderAddressDetails.LastName,
                order.OrderAddressDetails.Country,
                order.OrderAddressDetails.Landmark,
                order.OrderAddressDetails.State,
                order.OrderAddressDetails.City,
                order.OrderAddressDetails.PostalCode,
                order.OrderAddressDetails.Description
                );

            OrderModel obj = OrderModel.Create(
                OrderId.of(Guid.NewGuid()),
                ShippingAddress,
                OrderAddress,
                OrderName.of(order.OrderName),
                CustomerId.of(order.CustomerId),
                Payment.of(order.PaymentDetails.CardNumber, order.PaymentDetails.Cvv, order.PaymentDetails.ExpiryDate, order.PaymentDetails.PaymentType, order.PaymentDetails.CardMemberName),
                OrderStatus.Processing
                );

            order.OrderItems.ForEach(item =>
            {
                obj.AddItem(item.Quentity, ProductId.of(item.ProductId), item.Price);
            });
            obj.CalculatePrice();
            return obj;
        }
    }
}