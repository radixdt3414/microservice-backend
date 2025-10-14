using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using order.Application.Exceptions;
using order.Application.Order.Commands.OrderFailed;

namespace order.Application.Order.Commands.OrderedSuccessful
{
    public class OrderedSuccessfulCommandHandler(IApplicationDbContext dbContext, ILogger<OrderedSuccessfulCommandHandler> logger) : ICommandHandler<OrderedSuccessfulCommand, OrderedSuccessfulResponse>
    {
        public async Task<OrderedSuccessfulResponse> Handle(OrderedSuccessfulCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation($"--------------------order------> OrderedSuccessfulCommandHandler: OrderedSuccessfulCommand invoked.");
            var completedOrder = await dbContext.Orders.Where(x => x.Id == command.id && x.Status == Domain.Enum.OrderStatus.Processing).Include(x => x.OrderItems).FirstOrDefaultAsync();
            
            if (completedOrder == null)
            {
                logger.LogError($"--------------------order------> OrderedSuccessfulCommandHandler: Order not found.");
                throw new OrderNotFoundException(command.id.Value);
            }
            completedOrder!.Completed();
            logger.LogInformation($"--------------------order------> OrderedSuccessfulCommandHandler: order saved with completed status.");
            dbContext.Orders.Update(completedOrder);
            logger.LogInformation($"--------------------order------> OrderedSuccessfulCommandHandler: record updated.");
            await dbContext.SaveChangesAsync();
            return new OrderedSuccessfulResponse(true);
        }
    }
}
