namespace catelogs.API.Exceptions
{
    public class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(Guid Id): base(Id,"Product") { }
    }
}
