using discount.API.Model;
using Microsoft.EntityFrameworkCore;

namespace discount.API.Data
{
    public class DiscountContext : DbContext
    {
        public DiscountContext(DbContextOptions<DiscountContext> option) : base(option)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>().HasKey(c => c.Id);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Coupon> coupons { get; set; }
    }
}