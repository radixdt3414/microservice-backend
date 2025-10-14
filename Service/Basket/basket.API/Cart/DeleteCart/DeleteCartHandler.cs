using basket.API.Data.CartCacheStore;

namespace basket.API.Cart.DeleteCart
{
    public record DeleteCartCommand(string UserName) : ICommand<DeleteCartResponse>;
    public record DeleteCartResponse(bool IsSuccess);

    public class DeleteCartHandler(ICartItemRepository cartItemRepository, ICacheService<CartModel> cache) : ICommandHandler<DeleteCartCommand, DeleteCartResponse>
    {
        public async Task<DeleteCartResponse> Handle(DeleteCartCommand command, CancellationToken cancellationToken)
        {
            var cart = await cartItemRepository.GetCart(command.UserName);
            if (cart == null)
            {
                throw new CartNotFoundException(command.UserName);
            }
            await cartItemRepository.DeleteCart(cart);

            //Below cacheing functionality has already taken care by decortor pattern
            //await cache.Remove(command.UserName);
            return new DeleteCartResponse(true);
        }
    }
}
