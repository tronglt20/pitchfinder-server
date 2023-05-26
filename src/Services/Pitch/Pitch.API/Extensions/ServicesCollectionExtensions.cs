using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Grpc.Protos;
using Pitch.API.Services;
using Pitch.Infrastructure;
using Shared.Infrastructure.Dtos;

namespace Pitch.API.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddPitchDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PitchDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                options.UseLazyLoadingProxies();
            });

            // Database Migrations
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var servicesMigration = scope.ServiceProvider;
                var context = servicesMigration.GetRequiredService<PitchDbContext>();
                if (context.Database.GetPendingMigrations().Any())
                    context.Database.Migrate();
            }

            return services;
        }

        public static IServiceCollection AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
        {
            configuration.GetSection("GrpcSettings").Get<GrpcSettings>(options => options.BindNonPublicProperties = true);
            services.AddGrpcClient<OrderProtoService.OrderProtoServiceClient>(_ =>
            {
                _.Address = new Uri(GrpcSettings.OrderUrl);
            });

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddScoped<StoreService>()
                           .AddScoped<StoreOrderingService>()
                           .AddScoped<OrderGrpcService>();
        }
    }
}
