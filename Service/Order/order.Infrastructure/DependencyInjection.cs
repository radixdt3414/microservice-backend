using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using order.Application.Data;
using order.Infrastructure.Data;
using order.Infrastructure.Data.Interceptor;
using order.Infrastructure.Data.SeedData;

namespace order.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfigurationManager config)
        {
            services.AddScoped<ISaveChangesInterceptor, DomainEventDispatchInterceptor>();
            services.AddDbContext<OrderContext>((sp, option) =>
            {
                option.UseSqlServer(config.GetConnectionString("OrderDb"));
                option.AddInterceptors(sp.GetRequiredService<ISaveChangesInterceptor>());
            });
            services.AddScoped<IApplicationDbContext, OrderContext>();

            return services;
        }

        public static async Task  UseInfrastructureServices(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrderContext>();
            db.Database.MigrateAsync().GetAwaiter().GetResult();
            await SeedAsync(db);
        }

        public static async Task SeedAsync(OrderContext db)
        {
            await AddCustomerSeedDataAsync(db);
            //await AddProductSeedDataAsync(db);
            //await AddOrderAndOrderItemSeedDataAsync(db);
        }

        public static async Task AddCustomerSeedDataAsync(OrderContext db)
        {
            if (!await db.Customers.AnyAsync())
            {
                await db.Customers.AddRangeAsync(InitialData.CustomerData.ToArray());
                await db.SaveChangesAsync();
            }
        }

        public static async Task AddProductSeedDataAsync(OrderContext db)
        {
            if (!await db.Products.AnyAsync())
            {
                await db.Products.AddRangeAsync(InitialData.ProductData.ToArray());
                await db.SaveChangesAsync();
            }   
        }

        public static async Task AddOrderAndOrderItemSeedDataAsync(OrderContext db)
        {
            if(!await db.Orders.AnyAsync())
            {
                await db.Orders.AddRangeAsync(InitialData.OrderAndOrderItemData.ToArray());
                await db.SaveChangesAsync();
            }
        }
    }
}