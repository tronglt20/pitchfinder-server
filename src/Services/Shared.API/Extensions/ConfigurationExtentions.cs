using Microsoft.Extensions.Configuration;

namespace Shared.API.Extensions
{
    public static class ConfigurationExtentions
    {
        public static IConfiguration Build()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Load configuration from environment variables
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory);

            // Load configuration from ELDesk.Shared > appsettings.shared.json Files
            configBuilder.AddJsonFile("appsettings.shared.json", true, true)
                .AddJsonFile($"appsettings.shared.{environmentName}.json", true, true);

            // Load configuration from appsettings.json
            configBuilder.AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true);

            // Build and return
            return configBuilder
                .Build();
        }
    }
}