using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace buildingBlock.Messaging.Extension
{
    public static class Extensions
    {
        public static void AddMessageBroker(this IServiceCollection services, IConfigurationManager configuration, Assembly? assembly = null)
        {
            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();

                if(assembly != null)
                {
                    config.AddConsumers(assembly);
                }

                config.UsingRabbitMq((context, rabbitConfig) =>
                {
                    rabbitConfig.Host(new Uri(configuration["MessageBroker:Host"] ?? string.Empty), host =>
                    {
                        host.Username(configuration["MessageBroker:UserName"] ?? string.Empty);
                        host.Password(configuration["MessageBroker:Password"] ?? string.Empty);
                    });
                    rabbitConfig.ConfigureEndpoints(context);
                });

            });
        }
    }
}
