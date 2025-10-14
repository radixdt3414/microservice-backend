using inventory.Application.Data;
using inventory.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace inventory.Infrastructure.Data
{
    public class InventoryContext : DbContext, IApplicationDbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Warehouse> Warehouses => Set<Warehouse>();   
        public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();   
    }
}