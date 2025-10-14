using Microsoft.EntityFrameworkCore;
using order.Domain.Models;
using CustomerModel = order.Domain.Models.Customer;

namespace order.Application.Data
{
    public interface IApplicationDbContext
    {
        public DbSet<OrderModel> Orders { get; }
        public DbSet<OrderItem> OrderItems { get; }
        public DbSet<Product> Products { get; }
        public DbSet<CustomerModel> Customers { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
