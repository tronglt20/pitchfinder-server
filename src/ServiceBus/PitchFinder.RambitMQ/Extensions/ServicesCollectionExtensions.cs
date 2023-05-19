using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PitchFinder.RambitMQ.Handlers;

namespace PitchFinder.RambitMQ.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddRambitMQ(this IServiceCollection services
            , IConfiguration configuration)
        {
            var handlerCollection = new EventHandlerCollection(typeof(IntergrantionHandlerBase<>));
            services.AddMassTransit(_ =>
            {
                _.AddConsumers(handlerCollection.ToArray());
                _.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration["EventBusSettings:HostAddress"]);
                    cfg.ReceiveEndpoint($"{AppDomain.CurrentDomain.FriendlyName}-queue", c =>
                    {
                        c.ConfigureConsumers(ctx);
                    });
                });
            });
            services.AddScoped(_ => handlerCollection);

            return services;
        }
    }
}
