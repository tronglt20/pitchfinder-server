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
services.AddRedisCache(configuration);

// Config User claims info
services.AddUserInfo();

// Add gRPC clients
services.AddGrpcClients(configuration);

services
    .AddUnitOfWork<OrderDbContext>()
    .AddBaseRepositories()
    .AddServices();

// Add Utilities Services
services.AddRabbitMQ(configuration);

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

app.ConfigureDefault();

app.MapControllers();

app.Run();
