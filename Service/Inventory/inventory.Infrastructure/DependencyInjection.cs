using inventory.Application.Data;
using inventory.Infrastructure.Data;
using inventory.Infrastructure.Data.Interceptor;
using inventory.Infrastructure.Data.SeedData;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace inventory.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationManager configuration)
        {
            services.AddScoped<ISaveChangesInterceptor, DomainEventDispatchInterceptor>();
            services.AddDbContext<InventoryContext>((provider,config) =>
            {
                config.UseSqlServer(configuration.GetConnectionString("InventoryDb"));
                config.AddInterceptors(provider.GetRequiredService<ISaveChangesInterceptor>());

            });
            services.AddScoped<IApplicationDbContext, InventoryContext>();
            return services;
        }

        public static async Task UseInfrastructureServices(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<InventoryContext>();
            db.Database.Migrate();
            if (!db.Warehouses.Any())
            {
                db.Warehouses.AddRange(InitialData.lstWarehouses());
                await db.SaveChangesAsync();
            }
        }
    }
}