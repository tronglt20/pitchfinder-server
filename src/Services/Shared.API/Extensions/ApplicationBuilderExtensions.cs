namespace Shared.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void ConfigureDefault(this IApplicationBuilder app)
        {
            app.UseCors(builder => builder // Allow any
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
