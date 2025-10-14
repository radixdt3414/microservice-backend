using basket.API.Data.CartCacheStore;

namespace basket.API.Data.CacheCartRepository
{
    public class CacheCartRepository(ICartItemRepository cartRepository, ICacheService<CartModel> cache) : ICartItemRepository
    {
        public async Task AddCart(CartModel cart, CancellationToken cancellationToken)
        {
            await cartRepository.AddCart(cart, cancellationToken);
            await cache.Set(cart.UserName, cart);
        }

        public async Task DeleteCart(CartModel cart)
        {
            await cache.Remove(cart.UserName);
            await cartRepository.DeleteCart(cart);
        }

        public async Task<CartModel> GetCart(string UserName)
        {
            var obj = await cache.Get(UserName);
            if (obj == null)
            {
                obj = await cartRepository.GetCart(UserName);
                if(obj != null) await cache.Set(UserName, obj);
            }
            return obj;
        }

        public async Task UpdateCart(CartModel cart, CancellationToken cancellationToken)
        {
            await cache.Remove(cart.UserName);
            await cache.Set(cart.UserName, cart);
            await cartRepository.UpdateCart(cart, cancellationToken);
        }
    }
}