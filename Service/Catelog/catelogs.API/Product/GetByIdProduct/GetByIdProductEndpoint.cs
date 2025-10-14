using catelogs.API.Product.GetProduct;

namespace catelogs.API.Product.GetByIdProduct
{
    public record GetByIdProductResponseDTO(Guid Id, string Name, string Description, decimal Price, string Image, List<string> Categories);

    public class GetByIdProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/product/{Id}", async (Guid Id,IMediator _mediator) =>
            {
                var query = new GetByIdProductQuery(Id);
                var response = await _mediator.Send(query);
                var result = response.Adapt<GetByIdProductResponseDTO>();
                return Results.Ok(result);
            })
            .WithSummary("Get product by id")
            .WithDescription("get product by id")
            .Produces(200, typeof(GetProductResponseDTO))
            .ProducesProblem(400, null);
        }
    }
}