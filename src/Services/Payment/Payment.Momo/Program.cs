using Payment.Momo.Extensions;
using PitchFinder.RambitMQ.Extensions;
using Shared.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = ConfigurationExtentions.Build();

var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();

// Add Momo credential
services.AddMomoCredential(configuration);

// Add Services
services.AddServices();

// Add RabbitMQ
services.AddRabbitMQ(configuration);

services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
