using basket.API.Data.CartCacheStore;

namespace basket.API.Cart.GetCart
{
    public record GetCartQuery(string UserName) : IQuery<GetCartResponse>;
    public record GetCartResponse(CartModel cart);
    public class GetCartHandler(ICartItemRepository cartItemRepository, ICacheService<CartModel> cache) : IQueryHandler<GetCartQuery, GetCartResponse>
    {
        public async Task<GetCartResponse> Handle(GetCartQuery query, CancellationToken cancellationToken)
        {
            return new GetCartResponse(await cartItemRepository.GetCart(query.UserName));
        }
    }
}