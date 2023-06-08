using Payment.Momo.Services;

namespace Payment.Momo.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddScoped<MomoService>();
        }
        public static IServiceCollection AddMomoCredential(this IServiceCollection services, IConfiguration configuration)
        {
            configuration.GetSection("MomoCredential").Get<MomoCredential>(options => options.BindNonPublicProperties = true);
            return services;
        }
    }
}
