using IAM.API.Extensions;
using Shared.Service.Extensions;


var builder = WebApplication.CreateBuilder(args);
var configuration = ConfigurationExtentions.Build();

var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();

services.AddIAMDatabaseContext(configuration);

services.AddIAMIdentity();

// Add IdentityServer4
services.AddIdentityServer4(configuration);

// Config User claims info
services.AddUserInfo();

// Add Utilities Services
services.AddS3(configuration)
        .AddRambitMQ(configuration);

services.AddServices();

services.AddAuthenticationConfig(configuration)
        .AddAuthorizationConfig();

services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseIdentityServer();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
