namespace basket.API.Data.CartItemRepository
{
    public class CartItemRepository(IDocumentSession session) : ICartItemRepository
    {
        public async Task AddCart(CartModel cart, CancellationToken cancellationToken)
        {
            session.Store(cart);
            await session.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateCart(CartModel cart, CancellationToken cancellationToken)
        {
            session.Update(cart);
            await session.SaveChangesAsync(cancellationToken);
        }

        public async Task<CartModel> GetCart(string UserName)
        {
            var obj = await session.Query<CartModel>().Where(x=> x.UserName == UserName).FirstOrDefaultAsync();
            return obj;
        }

        public async Task DeleteCart(CartModel cart)
        {
            session.Delete<CartModel>(cart);
            await session.SaveChangesAsync();
        }
    }
}