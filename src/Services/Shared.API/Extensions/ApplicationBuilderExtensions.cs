using Microsoft.AspNetCore.Builder;

namespace Shared.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void ConfigureDefault(this IApplicationBuilder app)
        {
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()
                .WithExposedHeaders("*")
            );

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
