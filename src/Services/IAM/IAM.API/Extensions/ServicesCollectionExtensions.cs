using IAM.API.Identity;
using IAM.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IAM.API.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddIAMDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IAMDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            });

            // Database Migrations
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var servicesMigration = scope.ServiceProvider;
                var context = servicesMigration.GetRequiredService<IAMDbContext>();
                if (context.Database.GetPendingMigrations().Any())
                    context.Database.Migrate();
            }

            return services;
        }

        public static IServiceCollection AddIdentityServer4(this IServiceCollection services
            , IConfiguration configuration)
        {
            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddInMemoryClients(configuration.GetSection("IdentityServer:Clients"))
                    .AddInMemoryApiScopes(configuration.GetSection("IdentityServer:ApiScopes"))
                    .AddInMemoryIdentityResources(Config.GetIdentityResources())
				    .AddInMemoryApiResources(configuration.GetSection("IdentityServer:ApiResources"));

            return services;
        }
    }
}
