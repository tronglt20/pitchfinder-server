using Order.Grpc.Extensions;
using Order.Grpc.Services;
using Order.Infrastructure;
using Shared.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = ConfigurationExtentions.Build();
var services = builder.Services;

services.AddGrpc();

services.AddOrderDatabaseContext(configuration);
services.AddRedisCache(configuration);

services
    .AddUnitOfWork<OrderDbContext>()
    .AddBaseRepositories();

var app = builder.Build();

app.MapGrpcService<OrderGrpcService>();

app.Run();
