using Microsoft.EntityFrameworkCore;
using Pitch.Infrastructure;

namespace Pitch.Grpc.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPitchDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PitchDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                options.UseLazyLoadingProxies();
            });

            return services;
        }
    }
}
