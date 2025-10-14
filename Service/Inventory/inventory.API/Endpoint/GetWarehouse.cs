using Carter;
using inventory.Application.Stocks.Queries.GetWarehouse;
using inventory.Domain.DTOs;
using inventory.Domain.Models;
using Mapster;
using MediatR;

namespace inventory.API.Endpoint
{
    public record GetWarehouseResponseDTO(List<WarehouseDTO> warehouses);
    public class GetWarehouse : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("warehouse", async (ISender sender) =>
            {
                var query = new GetWarehouseQuery();
                var response = await sender.Send(query);
                return Results.Ok(new GetWarehouseResponseDTO( response.warehouses));
            })
           .WithSummary("Get warehouse")
           .WithDescription("Get warehouse")
           .Produces(200, typeof(GetWarehouseResponseDTO));
           
        }
    }
}
