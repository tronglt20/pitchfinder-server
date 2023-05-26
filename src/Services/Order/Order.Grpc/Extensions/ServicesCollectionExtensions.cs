using Microsoft.EntityFrameworkCore;
using Order.Infrastructure;

namespace Order.Grpc.Extensions
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

            return services;
        }

    }
}
