namespace order.Application.Exceptions
{
    public class OrderModificationFailedException : InternalServerException
    {
        public OrderModificationFailedException() : base("While modifing order encounter error.")  { }
    }
}