using Pitch.Grpc.Extensions;
using Pitch.Grpc.Services;
using Pitch.Infrastructure;
using Shared.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = ConfigurationExtentions.Build();

var services = builder.Services;

services.AddGrpc();

services.AddPitchDatabaseContext(configuration);
services.AddRedisCache(configuration);

services
    .AddUnitOfWork<PitchDbContext>()
    .AddBaseRepositories();

var app = builder.Build();

app.MapGrpcService<PitchGrpcService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
