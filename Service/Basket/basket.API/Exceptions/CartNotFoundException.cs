
namespace basket.API.Exceptions
{
    public class CartNotFoundException : NotFoundException
    {
        public CartNotFoundException(string UserName) : base(UserName, "Basket")
        {
        }
    }
}
