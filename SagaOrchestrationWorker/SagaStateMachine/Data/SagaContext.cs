using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using SagaStateMachine.Data.Configuration;

namespace SagaStateMachine.Data
{
    public class SagaContext : SagaDbContext
    {
        public SagaContext(DbContextOptions<SagaContext> options) : base(options) { }


        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get { yield return new OrderStateInstanceConfiguration(); }
        }
    }
}
