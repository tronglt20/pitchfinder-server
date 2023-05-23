using IAM.API.Identity;
using IAM.API.Services;
using IAM.Domain.Entities;
using IAM.Infrastructure;
using IAM.Infrastructure.Seender;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IAM.API.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddIAMIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<User, Role>(options =>
                {
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<IAMDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddIAMDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IAMDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                options.UseLazyLoadingProxies();
            });

            // Database Migrations
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var servicesMigration = scope.ServiceProvider;
                var context = servicesMigration.GetRequiredService<IAMDbContext>();
                if (context.Database.GetPendingMigrations().Any())
                    context.Database.Migrate();
            }

            // Seeding data here
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IAMDbContext>();
                IAMDataSeeder.SeedAsync(context);
            }

            return services;
        }

        public static IServiceCollection AddIdentityServer4(this IServiceCollection services
            , IConfiguration configuration)
        {
            services.AddIdentityServer(options =>
                    {
                        options.IssuerUri = configuration["IdentityServer:IssuerUri"];
                    })
                    .AddAspNetIdentity<User>()
                    .AddDeveloperSigningCredential()
                    .AddInMemoryClients(configuration.GetSection("IdentityServer:Clients"))
                    .AddInMemoryApiScopes(configuration.GetSection("IdentityServer:ApiScopes"))
                    .AddInMemoryIdentityResources(Config.GetIdentityResources())
                    .AddInMemoryApiResources(configuration.GetSection("IdentityServer:ApiResources"))
                    .AddProfileService<ProfileService>()
                    .AddInMemoryPersistedGrants()
                    .AddDeveloperSigningCredential();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                    .AddScoped<AuthenticationService>();
        }
    }
}
