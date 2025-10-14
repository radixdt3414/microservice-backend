using buildingBlock.CQRS;
using inventory.Application.Data;
using inventory.Application.Extension;
using inventory.Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace inventory.Application.Stocks.Queries.GetWarehouse
{
    public record GetWarehouseQuery() : IQuery<GetWarehouseResponse>;
    public record GetWarehouseResponse(List<WarehouseDTO> warehouses);

    public class GetWarehouseHandler(IApplicationDbContext dbContext, ILogger<GetWarehouseHandler> logger) : IQueryHandler<GetWarehouseQuery, GetWarehouseResponse>
    {
        public async Task<GetWarehouseResponse> Handle(GetWarehouseQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation($"--------------------inventory------> GetWarehouseHandler: GetWarehouseHandler invoked.");

            var result = await dbContext.Warehouses.AsNoTracking().Include(x => x.InventoryItem).ToListAsync();

            logger.LogInformation($"--------------------inventory------> GetWarehouseHandler: Query executed successfully.");

            return new GetWarehouseResponse(Converter.WarehouseModelToDTO(result));
        }
    }
}
