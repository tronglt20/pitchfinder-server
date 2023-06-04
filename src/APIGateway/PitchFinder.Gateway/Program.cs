using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
        .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);

// Config cross origin
builder.Services.AddCors();

// Add Ocelot
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();
app.UseRouting();

// global cors policy
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
                                        //.AllowCredentials()
    .WithExposedHeaders("*")
); // allow credentials

await app.UseOcelot();

app.Run();

