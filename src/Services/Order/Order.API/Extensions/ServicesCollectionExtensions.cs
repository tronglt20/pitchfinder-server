using Microsoft.EntityFrameworkCore;
using Order.Infrastructure;

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


        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
