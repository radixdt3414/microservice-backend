namespace basket.API.Cart.GetCart
{   public record GetCartResponseDTO(CartModel cart);
    public class GetCartEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("basket", async (string UserName,ISender sender) =>
            {
                var req = new GetCartQuery(UserName);
                //var response = 
                var result = await sender.Send(req);
                return Results.Ok(result);
            });
        }
    }
}