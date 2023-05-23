using Order.API.Extensions;
using Order.Infrastructure;
using PitchFinder.RambitMQ.Extensions;
using Shared.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = ConfigurationExtentions.Build();
var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();

services.AddOrderDatabaseContext(configuration);

// Config User claims info
services.AddUserInfo();

services
    .AddUnitOfWork<OrderDbContext>()
    .AddBaseRepositories()
    .AddServices();

// Add Utilities Services
services.AddRambitMQ(configuration);

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