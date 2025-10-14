using catelogs.API.Product.GetProduct;
using Microsoft.AspNetCore.Mvc;

namespace catelogs.API.Product.GetProductByCategory
{
    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public record GetProductByCategoryResponseDTO(IEnumerable<ProductModel> LstProduct);
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/product/category/{category}", async (string category, IMediator _mediator) =>
            {
                var query = new GetProductByCategoryQuery(category);
                var response = await _mediator.Send(query);
                var result = response.Adapt<GetProductByCategoryResponseDTO>();
                return Results.Ok(result);
            })
            .WithSummary("Get product by id")
            .WithDescription("get product by id")
            .Produces(200, typeof(GetProductResponseDTO))
            .ProducesProblem(400, null);
        }
    }
}