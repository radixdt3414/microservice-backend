using Carter;
using inventory.Application.Stocks.Commands.AddInventory;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace inventory.API.Endpoint
{
    public record AddInventoryRequest(
        Guid? Id,
        Guid ProductId,
        long Quantity,
        Guid WarehouseId);

    public record AddInventoryResponseDTO(bool IsSuccess);

    public class AddInventoryItem : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/warehouse", async ([FromBody] AddInventoryRequest request, ISender _mediator) =>
            {
                var command = request.Adapt<AddInventoryCommand>();
                var response = await _mediator.Send(command);
                var result = response.Adapt<AddInventoryResponseDTO>();
                if(result.IsSuccess )
                {
                    return Results.Ok("Warehouse updated successfully.");
                }
                else
                {
                    return Results.Problem(
                            title: "Warehouse update process failed.",
                            detail: "While updating warehouse error encountered.",
                            statusCode: StatusCodes.Status500InternalServerError
                            );
                }
            })
           .WithSummary("Add update warehouse")
           .WithDescription("Add update warehouse")
           .Produces(200, typeof(AddInventoryResponseDTO))
           .ProducesProblem(400, null);
        }
    }
}
