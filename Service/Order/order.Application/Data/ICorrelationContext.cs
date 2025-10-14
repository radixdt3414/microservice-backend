namespace order.Application.Data
{
    public interface ICorrelationContext
    {
        public Guid CorrelationId {  get; set; }
    }
}
