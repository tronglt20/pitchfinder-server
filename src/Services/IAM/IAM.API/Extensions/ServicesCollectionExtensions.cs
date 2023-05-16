using IAM.API.Identity;

namespace IAM.API.Extensions
{
    public static class ServicesCollectionExtensions
    {
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
