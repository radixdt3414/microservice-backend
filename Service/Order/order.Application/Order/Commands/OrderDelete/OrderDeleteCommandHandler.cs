using order.Application.Exceptions;
using order.Domain.ValueObjects;

namespace order.Application.Order.Commands.OrderDelete
{
    public class OrderDeleteCommandHandler(IApplicationDbContext DbContext) : ICommandHandler<OrderDeleteCommand, OrderDeleteResponse>
    {
        public async Task<OrderDeleteResponse> Handle(OrderDeleteCommand command, CancellationToken cancellationToken)
        {
            var orderId = OrderId.of(command.Id);
            var order = await DbContext.Orders.FindAsync([orderId]);
            if (order == null)
            {
                throw new OrderNotFoundException(orderId.Value);
            }
            DbContext.Orders.Remove(order);
            await DbContext.SaveChangesAsync(cancellationToken);
            return new OrderDeleteResponse(true);
        }
    }
}
