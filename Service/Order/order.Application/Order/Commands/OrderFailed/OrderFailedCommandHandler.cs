using Microsoft.Extensions.Logging;
using order.Application.Exceptions;

namespace order.Application.Order.Commands.OrderFailed
{
    internal class OrderFailedCommandHandler(IApplicationDbContext dbContext, ILogger<OrderFailedCommandHandler> logger) : ICommandHandler<OrderFailedCommand, OrderFailedResponse>
    {
        public async Task<OrderFailedResponse> Handle(OrderFailedCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation($"--------------------order------> OrderFailedCommandHandler: order Id {command.Id}.");
            var temp = await dbContext.Orders.Where(x => x.Id == command.Id).Include(x => x.OrderItems).FirstOrDefaultAsync();
            logger.LogInformation($"--------------------order------> OrderFailedCommandHandler: is order available: {temp != null}.");
            logger.LogInformation($"--------------------order------> OrderFailedCommandHandler: order status: {temp?.Status.ToString()}.");

            var failedOrder = await dbContext.Orders.Where(x => x.Id == command.Id && x.Status == Domain.Enum.OrderStatus.Processing).Include(x => x.OrderItems).FirstOrDefaultAsync();
            if (failedOrder == null)
            {
                logger.LogInformation($"--------------------order------> OrderFailedCommandHandler: Order not found.");
                throw new OrderNotFoundException(command.Id.Value);
            }
            failedOrder!.Failed();
            logger.LogInformation($"--------------------order------> OrderFailedCommandHandler: status updated to failed.");
            dbContext.Orders.Update(failedOrder);
            logger.LogInformation($"--------------------order------> OrderFailedCommandHandler: record saved.");
            await dbContext.SaveChangesAsync();
            return new OrderFailedResponse { IsSuccess = true };
        }
    }
}