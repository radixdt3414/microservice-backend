namespace basket.API.Data.CartItemRepository
{
    public interface ICartItemRepository
    {
        public Task AddCart(CartModel cart, CancellationToken cancellationToken);
        public Task UpdateCart(CartModel cart, CancellationToken cancellationToken);
        public Task<CartModel> GetCart(string UserName);
        Task DeleteCart(CartModel cart);
    }
}