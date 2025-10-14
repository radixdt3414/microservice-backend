using Carter;
using inventory.API.Endpoint;

namespace inventory.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAPI(this IServiceCollection services)
        {
            services.AddCarter(null, config => config.WithModule<GetWarehouse>());
            services.AddCarter(null, config => config.WithModule<AddInventoryItem>());
            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.MapCarter();
            return app;
        }
    }
}
