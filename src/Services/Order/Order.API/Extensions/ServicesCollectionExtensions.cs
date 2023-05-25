using Microsoft.EntityFrameworkCore;
using Order.API.Services;
using Order.Infrastructure;
using Pitch.Grpc.Protos;
using Shared.Infrastructure.Dtos;

namespace Order.API.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddOrderDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                options.UseLazyLoadingProxies();
            });

            // Database Migrations
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var servicesMigration = scope.ServiceProvider;
                var context = servicesMigration.GetRequiredService<OrderDbContext>();
                if (context.Database.GetPendingMigrations().Any())
                    context.Database.Migrate();
            }

            return services;
        }

        public static IServiceCollection AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
        {
            configuration.GetSection("GrpcSettings").Get<GrpcSettings>(options => options.BindNonPublicProperties = true);
            services.AddGrpcClient<PitchProtoService.PitchProtoServiceClient>(_ =>
            {
                _.Address = new Uri(GrpcSettings.PitchUrl);
            });

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddScoped<OrderService>()
                           .AddScoped<PitchGrpcService>();
        }
    }
}
