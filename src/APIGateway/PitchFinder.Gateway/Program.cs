using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
        .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);

// Add Ocelot
builder.Services.AddOcelot(builder.Configuration);

// Add Cors
builder.Services.AddCors();

var app = builder.Build();
app.UseRouting();

app.UseCors(builder => builder // Allow any
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

await app.UseOcelot();

app.Run();

