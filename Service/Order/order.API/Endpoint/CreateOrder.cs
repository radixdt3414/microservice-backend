using Microsoft.AspNetCore.Mvc;
using order.Application.Order.Commands.OrderCreate;

namespace order.API.Endpoint
{
    public record CreateOrderRequestDTO(OrderDTO Order);
    public record CreateOrderResponseDTO(Guid OrderId);

    public class CreateOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/order", async ([FromBody]CreateOrderRequestDTO request, ISender _mediator) =>
            {
                var command = request.Adapt<OrderCreateCommand>();
                var response = await _mediator.Send(command);
                var result = response.Adapt<CreateOrderResponseDTO>();
                return Results.Created($"/order/{result.OrderId}", result);
            })
            .WithSummary("Create order")
            .WithDescription("Create order")
            .Produces(201, typeof(CreateOrderResponseDTO))
            .ProducesProblem(400, null);
        }
    }
}