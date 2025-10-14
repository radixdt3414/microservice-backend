using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using StateMachine.Data.Configuration;
using StateMachine.StateInstance;

namespace StateMachine.Data
{
    public class SagaContext : DbContext
    {
        public SagaContext(DbContextOptions<SagaContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderInstance>().HasKey(x => x.CorrelationId);
        }

        public DbSet<OrderInstance> OrderInstances { get; set; }
        //protected override IEnumerable<ISagaClassMap> Configurations
        //{
        //    get { yield return new OrderStateInstanceConfiguration(); }
        //}
    }
}
