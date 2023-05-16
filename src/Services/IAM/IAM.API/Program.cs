using IAM.API.Extensions;
using Shared.Service.Extensions;


var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();

services.AddIAMDatabaseContext(builder.Configuration);

services.AddIAMIdentity();

// Add IdentityServer4
services.AddIdentityServer4(builder.Configuration);

// Config User claims info
services.AddUserInfo();

services.AddServices();

services.AddAuthenticationConfig(builder.Configuration);
services.AddAuthorizationConfig();

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
