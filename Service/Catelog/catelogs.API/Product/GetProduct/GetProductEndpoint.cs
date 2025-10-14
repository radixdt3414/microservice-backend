namespace catelogs.API.Product.GetProduct
{
    public record GetProductResponseDTO(Guid Id , string Name, string Description, decimal Price, string Image, List<string> Categories);

    public class GetProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async (PaginationDTO request, IMediator _mediator) =>
            {
                var query = new GetProductQuery(request);
                var response = await _mediator.Send(query);
                //var result = response.Adapt<PageResultDTO<List<GetProductResponseDTO>>>();
                return Results.Ok(response);
            })
            .WithSummary("Get product list")
            .WithDescription("get product list")
            .Produces(200, typeof(GetProductResponseDTO))
            .ProducesProblem(400, null);
        }
    }
}
