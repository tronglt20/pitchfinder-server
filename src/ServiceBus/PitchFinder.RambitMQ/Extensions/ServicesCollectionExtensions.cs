using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PitchFinder.RambitMQ.Events;

namespace PitchFinder.RambitMQ.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddRambitMQ(this IServiceCollection services
            , IConfiguration configuration
            , Type implementAssemblyType)
        {
            var eventCollection = new IntegrationEventCollection(implementAssemblyType);
            services.AddScoped(_ => eventCollection);
            services.AddMassTransit(_ =>
            {
                _.AddConsumers(eventCollection.ToArray());
                _.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration["EventBusSettings:HostAddress"]);
                    cfg.ReceiveEndpoint("pitchfinder-queue", c =>
                    {
                        c.ConfigureConsumers(ctx);
                    });
                });
            });

            return services;
        }
    }
}
