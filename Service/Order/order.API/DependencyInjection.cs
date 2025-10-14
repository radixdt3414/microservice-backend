using order.API.Endpoint;
using System.Reflection;

namespace order.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddMediatR(option =>
            {
                option.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                //option.AddBehavior(typeof(ValidationBehaviour<,>));
                //option.AddBehavior(typeof(LoggingBehaviour<,>));
            });
            services.AddCarter(null, config => config.WithModule<CreateOrder>());
            services.AddCarter(null, config => config.WithModule<UpdateOrder>());
            services.AddCarter(null, config => config.WithModule<DeleteOrder>());
            services.AddCarter(null, config => config.WithModule<GetOrder>());
            services.AddCarter(null, config => config.WithModule<GetOrderByCustomer>());
            services.AddCarter(null, config => config.WithModule<GetOrderByName>());
            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.MapCarter();
            return app;
        }
    }
}
