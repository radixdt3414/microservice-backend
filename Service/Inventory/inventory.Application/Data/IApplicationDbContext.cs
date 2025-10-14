using inventory.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace inventory.Application.Data
{
    public interface IApplicationDbContext
    {
        public DbSet<Warehouse> Warehouses { get;}
        public DbSet<InventoryItem> InventoryItems { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }   
}