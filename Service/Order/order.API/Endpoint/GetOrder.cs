using buildingBlock.DTO;
using Microsoft.AspNetCore.Mvc;
using order.Application.Order.Queries.GetOrder;

namespace order.API.Endpoint
{
    public record GetOrderRequestDTO(PaginationDTO PageDetails) { }
    public record GetOrderResponseDTO(PageResultDTO<IEnumerable<OrderDTO>> OrderList) { }

    public class GetOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/orders", async ([FromBody] GetOrderRequestDTO request, ISender _mediator) =>
            {
                if(request == null)
                {
                    return Results.BadRequest("Page details should not be null.");
                }
                var command = request.Adapt<GetOrderQuery>();
                var response = await _mediator.Send(command);
                var result = response.Adapt<GetOrderResponseDTO>();
                return Results.Ok(result);
            })
            .WithSummary("Get order")
            .WithDescription("Get order")
            .Produces(200, typeof(GetOrderResponseDTO))
            .ProducesProblem(400, null);
        }
    }
}
