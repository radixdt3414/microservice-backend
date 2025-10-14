using Microsoft.EntityFrameworkCore;
using order.Application.Data;
using order.Domain.Models;

namespace order.Infrastructure.Data
{
    public class OrderContext : DbContext, IApplicationDbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options):base(options) { }
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Customer> Customers => Set<Customer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderContext).Assembly);   
            base.OnModelCreating(modelBuilder);
        }
    }
}
