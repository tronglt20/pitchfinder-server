using MassTransit;
using Microsoft.EntityFrameworkCore;
using Pitch.API.Services;
using Pitch.Infrastructure;

namespace Pitch.API.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddPitchDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PitchDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
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

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddScoped<StoreService>();
        }
    }
}
