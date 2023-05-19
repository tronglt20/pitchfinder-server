using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PitchFinder.RambitMQ.Handlers;

namespace PitchFinder.RambitMQ.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddRambitMQ(this IServiceCollection services
            , IConfiguration configuration
            , Type implementAssemblyType)
        {
            var handlerCollection = new EventHandlerCollection(implementAssemblyType);
            services.AddScoped(_ => handlerCollection);
            services.AddMassTransit(_ =>
            {
                _.AddConsumers(handlerCollection.ToArray());
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
