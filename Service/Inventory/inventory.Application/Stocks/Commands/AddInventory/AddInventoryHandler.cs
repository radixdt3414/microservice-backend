using buildingBlock.CQRS;
using FluentValidation;
using inventory.Application.Data;
using inventory.Application.Exceptions;
using inventory.Application.Stocks.Queries.GetWarehouse;
using inventory.Domain.Models;
using inventory.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace inventory.Application.Stocks.Commands.AddInventory
{
    public record AddInventoryCommand(
        Guid? Id,
        Guid ProductId,
        long Quantity,
        Guid WarehouseId) : ICommand<AddInventoryResponse>;

    public record AddInventoryResponse(bool IsSuccess);

    public class AddInventoryCommandValidator : AbstractValidator<AddInventoryCommand>
    {
        public AddInventoryCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product Id is required");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
            RuleFor(x => x.WarehouseId).NotEmpty().WithMessage("Warehouse Id is required");
        }
    }

    public class AddInventoryHandler(IApplicationDbContext dbContext, ILogger<AddInventoryHandler> logger) : ICommandHandler<AddInventoryCommand, AddInventoryResponse>
    {
        public async Task<AddInventoryResponse> Handle(AddInventoryCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation($"--------------------inventory------> AddInventoryHandler: AddInventoryHandler invoked.");
            var selectedWarehouse = await dbContext.Warehouses.Where(x => x.Id == WarehouseId.Of( command.WarehouseId)).Include(x => x.InventoryItem).FirstOrDefaultAsync();
            if (selectedWarehouse == null) {
                logger.LogError($"--------------------inventory------> AddInventoryHandler: Warehouse not found.");
                throw new WarehouseNotFoundException(command.WarehouseId); 
            }
            if(command.Id ==  Guid.Empty || command.Id == null)
            {
                logger.LogError($"--------------------inventory------> AddInventoryHandler: Added new item to warehouse.");
                selectedWarehouse!.AddInventoryItem(ProductId.Of(command.ProductId), (int)command.Quantity, 0);
                
            }
            else
            {
                logger.LogError($"--------------------inventory------> AddInventoryHandler: updated item in warehouse.");
                selectedWarehouse!.UpdateInventoryItem(ProductId.Of(command.ProductId), (int)command.Quantity);
                
            }
            dbContext.Warehouses.Update(selectedWarehouse);
            await dbContext.SaveChangesAsync();
            logger.LogError($"--------------------inventory------> AddInventoryHandler: changes in warehouse saved.");
            return new AddInventoryResponse(true);
        }
    }
}
