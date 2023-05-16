using IAM.API.Extensions;
using Shared.Service.Extensions;


var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddControllers();
services.AddEndpointsApiExplorer();

// Add Database Connection
services.AddIAMDatabaseContext(builder.Configuration);

// Add Authentication config
services.AddAuthenticationConfig();
// Add Authorization config
services.AddAuthorizationConfig();

// Config User claims info
services.AddUserInfo();

// Add IdentityServer4
services.AddIdentityServer4(builder.Configuration);


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
