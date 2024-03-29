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

app.Run();
