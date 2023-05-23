using Pitch.API.Extensions;
using Pitch.Infrastructure;
using PitchFinder.RambitMQ.Extensions;
using Shared.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = ConfigurationExtentions.Build();

var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();

services.AddPitchDatabaseContext(configuration);
services.AddRedisCache(configuration);

// Config User claims info
services.AddUserInfo();

services
    .AddUnitOfWork<PitchDbContext>()
    .AddBaseRepositories()
    .AddServices();

// Add Utilities Services
services.AddS3(configuration)
        .AddRambitMQ(configuration);

services.AddAuthenticationConfig(configuration)
        .AddAuthorizationConfig();

services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
