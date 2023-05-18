using Pitch.API.Extensions;
using Shared.Service.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = ConfigurationExtentions.Build();

var services = builder.Services;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

services.AddPitchDatabaseContext(configuration);

// Config User claims info
services.AddUserInfo();

// Add S3
services.AddS3Service(configuration);

services.AddServices();

services.AddAuthenticationConfig(configuration)
        .AddAuthorizationConfig();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
