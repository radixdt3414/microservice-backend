using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SagaStateMachine.StateInstance;

namespace SagaStateMachine.Data.Configuration
{
    public class OrderStateInstanceConfiguration : SagaClassMap<OrderInstance>
    {
        protected override void Configure(EntityTypeBuilder<OrderInstance> entity, ModelBuilder model)
        {
            entity.HasKey(x => x.CorrelationId);
            entity.Property(x => x.CorrelationId);
            entity.Property(x => x.UserName);
            entity.Property(x => x.CurrentState);
            entity.Property(x => x.CustomerId);
        }
    }
}
