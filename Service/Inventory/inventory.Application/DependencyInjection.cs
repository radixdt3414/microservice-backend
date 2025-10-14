using buildingBlock.Behaviour;
using buildingBlock.Messaging.Extension;
using inventory.Application.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace inventory.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfigurationManager configuration)
        {
            services.AddScoped<ICorrelationContext, CorrelationContext>();
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
                config.AddOpenBehavior(typeof(LoggingBehaviour<,>));

            });
            services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());
            return services;
        }
    }
}