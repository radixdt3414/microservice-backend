using buildingBlock.Messaging.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System.Reflection;

namespace order.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfigurationManager config)
        {
            services.AddScoped<ICorrelationContext, CorrelationContext>();
            services.AddFeatureManagement();
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
                config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
            });
            services.AddMessageBroker(config, Assembly.GetExecutingAssembly());
            
            return services;
        }
    }
}
