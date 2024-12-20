using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddOcelot(builder.Configuration);
const string AuthenticationProviderKey = "MyKey";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(AuthenticationProviderKey, x =>
    {
        x.Authority = "https://cognito-idp.eu-west-2.amazonaws.com/eu-west-2_peIhKxYwJ";
        x.MetadataAddress = "https://cognito-idp.eu-west-2.amazonaws.com/eu-west-2_peIhKxYwJ/.well-known/openid-configuration";
        x.IncludeErrorDetails = true;
        x.RequireHttpsMetadata = false;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });
builder.Services.AddAuthorizationBuilder();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.UseOcelot();
app.UseHttpsRedirection();

app.Run();