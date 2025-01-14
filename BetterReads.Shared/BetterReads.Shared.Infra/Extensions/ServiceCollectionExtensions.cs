using Azure.Identity;
using BetterReads.Shared.Infra.Repositories;
using BetterReads.Shared.Infra.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BetterReads.Shared.Infra.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongo(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.Configure<MongoSettings>(configuration.GetSection(MongoSettings.Name));
        services.AddSingleton(typeof(IMongoRepository<,>), typeof(MongoRepository<,>));

        return services;
    }

    public static IServiceCollection AddKeyVault(this IServiceCollection services,
        IConfigurationManager configurationManager)
    {
        var keyVaultUrl = new Uri(configurationManager.GetSection("KeyVault").GetSection("Url").Value!);
        var clientId = configurationManager.GetSection("KeyVault").GetSection("ClientId").Value!;
        var clientSecret = configurationManager.GetSection("KeyVault").GetSection("ClientSecret").Value!;
        var directoryId = configurationManager.GetSection("KeyVault").GetSection("DirectoryId").Value!;

        var credential = new ClientSecretCredential(directoryId, clientId, clientSecret);
        configurationManager.AddAzureKeyVault(keyVaultUrl, credential);
        return services;
    }

    public static IServiceCollection AddCognitoJwtAuth(this IServiceCollection services,
        IConfigurationManager configurationManager)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                x.Authority = configurationManager["Cognito:Authority"];
                x.MetadataAddress = configurationManager["Cognito:MetadataAddress"]!;
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
        services.AddAuthorizationBuilder();
        
        return services;
    }
}