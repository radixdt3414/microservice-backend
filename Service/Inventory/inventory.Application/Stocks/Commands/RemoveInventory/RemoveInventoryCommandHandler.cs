using buildingBlock.CQRS;
using FluentValidation;
using inventory.Application.Data;
using inventory.Application.Stocks.Commands.AddInventory;
using inventory.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace inventory.Application.Stocks.Commands.RemoveInventory
{

    public record RemoveInventoryCommand(Guid Id) : ICommand<RemoveInventoryResponse>;
    public record RemoveInventoryResponse(bool IsSuccess);

    public class RemoveInventoryCommandValidator : AbstractValidator<RemoveInventoryCommand>
    {
        public RemoveInventoryCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        }
    }
    public class RemoveInventoryCommandHandler(IApplicationDbContext dbContext, ILogger<AddInventoryHandler> logger) : ICommandHandler<RemoveInventoryCommand, RemoveInventoryResponse>
    {
        public async Task<RemoveInventoryResponse> Handle(RemoveInventoryCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation($"--------------------inventory------> RemoveInventoryCommandHandler: RemoveInventoryCommandHandler invoked.");
            var lstWarehouse = dbContext.Warehouses.Include(x => x.InventoryItem).ToList();
            foreach (var item in lstWarehouse)
            {
                item.RemoveInventoryItem(ProductId.Of(command.Id));
            }
            dbContext.Warehouses.UpdateRange(lstWarehouse);
            logger.LogInformation($"--------------------inventory------> RemoveInventoryCommandHandler: warehouse records updated.");
            await dbContext.SaveChangesAsync();
            logger.LogInformation($"--------------------inventory------> RemoveInventoryCommandHandler: Updated warehouse records saved.");
            return new RemoveInventoryResponse(true);
        }
    }
}
