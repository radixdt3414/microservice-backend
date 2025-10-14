using Microsoft.AspNetCore.Mvc;
using order.Application.Order.Queries.GetOrderByCustomer;
using order.Application.Order.Queries.GetOrderByName;

namespace order.API.Endpoint
{
    public record GetOrderByNameRequestDTO{ 
        public string name { get; set; }
    }
    public record GetOrderByNameResponseDTO(IEnumerable<OrderDTO> OrderList) { }

    public class GetOrderByName : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/order/name", async ([FromBody]GetOrderByNameRequestDTO request, ISender _mediator) =>
            {
                var qry = new GetOrderByNameQuery(request.name);
                var response = await _mediator.Send(qry);
                var result = response.Adapt<GetOrderByNameResponseDTO>();
                return Results.Ok(result);
            })
            .WithSummary("Get order by name")
            .WithDescription("Get order by name")
            .Produces(200, typeof(GetOrderByNameResponseDTO))
            .ProducesProblem(400, null);
        }
    }
}
