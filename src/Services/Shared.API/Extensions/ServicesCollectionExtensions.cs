using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.API.Identity;
using Shared.Domain.Enums;
using Shared.Domain.Interfaces;
using Shared.Infrastructure;
using Shared.Infrastructure.Dtos;

namespace Shared.API.Extensions
{
    public static partial class ServicesCollectionExtensions
    {
        public static IServiceCollection AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection("IdentityOptions").Get<IdentitySettings>();
            services.AddSingleton(provider => settings);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = settings.Authority;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

            return services;
        }

        public static IServiceCollection AddAuthorizationConfig(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyNames.OWNER_API, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("RoleId", ((int)RoleEnum.Owner).ToString());
                });

                options.AddPolicy(PolicyNames.Customer_API, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("RoleId", ((int)RoleEnum.Customer).ToString());
                });
            });

            return services;
        }

        public static void AddUserInfo(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped(serviceProvider =>
            {
                var httpContext = serviceProvider.GetService<IHttpContextAccessor>().HttpContext;
                return httpContext?.CurrentUser();
            });
        }

        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services) where TContext : DbContext
        {
            return services
                .AddScoped(typeof(IUnitOfWorkBase<>), typeof(UnitOfWorkBase<>))
                .AddScoped(typeof(IUnitOfWorkBase), typeof(UnitOfWorkBase<TContext>));
        }

        public static IServiceCollection AddBaseRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>))
                .AddImplementationInterfaces(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        }

        public static IServiceCollection AddImplementationInterfaces(this IServiceCollection services
           , Type interfaceType
           , Type implementAssemblyType)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var derivedTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(implementAssemblyType) ||
                               (implementAssemblyType.IsGenericTypeDefinition &&
                                type.BaseType != null &&
                                type.BaseType.IsGenericType &&
                                type.BaseType.GetGenericTypeDefinition() == implementAssemblyType));

            foreach (var implementType in derivedTypes)
            {
                var mainInterfaces = implementType
                    .GetInterfaces()
                    .Where(_ => _.GenericTypeArguments.Count() == 0);

                foreach (var mainInterface in mainInterfaces)
                {
                    services.AddScoped(mainInterface, implementType);
                }
            }

            return services;
        }
    }
}
