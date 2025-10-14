using basket.API.Cart.GetCart;

namespace basket.API.Cart.DeleteCart
{
    public record DeleteCartResponseDTO(bool IsSuccess);
    public class DeleteCartEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket", async (string UserName, ISender sender) =>
            {
                var req = new DeleteCartCommand(UserName);
                var response = await sender.Send(req);
                return Results.Ok( response.Adapt<DeleteCartResponseDTO>());
            });
        }
    }
}
