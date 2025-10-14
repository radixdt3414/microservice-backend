using order.Application.Exceptions;
using order.Domain.ValueObjects;

namespace order.Application.Order.Commands.OrderUpdate
{
    public class OrderUpdateCommandHandler(IApplicationDbContext DbContext) : ICommandHandler<OrderUpdateCommand, OrderUpdateResponse>
    {
        public async Task<OrderUpdateResponse> Handle(OrderUpdateCommand command, CancellationToken cancellationToken)
        {
            var id = command.Order?.Id ?? Guid.NewGuid();
            var orderId = OrderId.of(id);
            var order = await DbContext.Orders.FindAsync([orderId]);
            if(order == null)
            {
                throw new OrderNotFoundException(orderId.Value);
            }
            OrderModel obj = UpdateOrder(order, command.Order);
            DbContext.Orders.Update(obj!);
            await DbContext.SaveChangesAsync(cancellationToken);
            return new OrderUpdateResponse(true);
        }

        public OrderModel UpdateOrder(OrderModel ExistingOrder, OrderDTO UpdatedOrder)
        {
            Address ShippingAddress = Address.of(
                UpdatedOrder.ShippingAddressDetails.FirstName,
                UpdatedOrder.ShippingAddressDetails.LastName,
                UpdatedOrder.ShippingAddressDetails.Country,
                UpdatedOrder.ShippingAddressDetails.Landmark,
                UpdatedOrder.ShippingAddressDetails.State,
                UpdatedOrder.ShippingAddressDetails.City,
                UpdatedOrder.ShippingAddressDetails.PostalCode,
                UpdatedOrder.ShippingAddressDetails.Description
                );


            var OrderAddress = Address.of(
                UpdatedOrder.OrderAddressDetails.FirstName,
                UpdatedOrder.OrderAddressDetails.LastName,
                UpdatedOrder.OrderAddressDetails.Country,
                UpdatedOrder.OrderAddressDetails.Landmark,
                UpdatedOrder.OrderAddressDetails.State,
                UpdatedOrder.OrderAddressDetails.City,
                UpdatedOrder.OrderAddressDetails.PostalCode,
                UpdatedOrder.OrderAddressDetails.Description
                );

            var PaymentDetails = Payment.of(
                UpdatedOrder.PaymentDetails.CardNumber, 
                UpdatedOrder.PaymentDetails.Cvv, 
                UpdatedOrder.PaymentDetails.ExpiryDate, 
                UpdatedOrder.PaymentDetails.PaymentType,
                UpdatedOrder.PaymentDetails.CardMemberName
                );

            ExistingOrder.Update(ShippingAddress, OrderAddress, OrderName.of(UpdatedOrder.OrderName), CustomerId.of(UpdatedOrder.CustomerId), PaymentDetails);
            return ExistingOrder;
        }
    }
}
