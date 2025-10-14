
namespace order.Application.Data
{
    public class CorrelationContext : ICorrelationContext
    {
        public Guid CorrelationId { get; set; }
    }
}
