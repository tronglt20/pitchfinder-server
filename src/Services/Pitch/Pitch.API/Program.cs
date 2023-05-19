using Pitch.API.Extensions;
using Shared.API.Extensions;
using PitchFinder.RambitMQ.Extensions;
using PitchFinder.RambitMQ.Handlers;
using Pitch.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = ConfigurationExtentions.Build();

var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();

services.AddPitchDatabaseContext(configuration);

// Config User claims info
services.AddUserInfo();

// Config User claims info
services.AddUserInfo();

// Add Utilities Services
services.AddS3(configuration)
        .AddRambitMQ(configuration, typeof(IntergrantionHandlerBase<>));

services
    .AddUnitOfWork<PitchDbContext>()
    .AddBaseRepositories()
    .AddServices();

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
