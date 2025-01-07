using Azure.Identity;
using BetterReads.Shared.Infra.Repositories;
using BetterReads.Shared.Infra.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BetterReads.Shared.Infra.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongo(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.Configure<MongoSettings>(configuration.GetSection(MongoSettings.Name));
        services.AddSingleton(typeof(IMongoRepository<,>), typeof(MongoRepository<,>));

        return services;
    }

    public static IServiceCollection AddKeyVault(this IServiceCollection services, IConfigurationManager configurationManager)
    {
        var keyVaultUrl = new Uri(configurationManager.GetSection("KeyVault").GetSection("Url").Value!);
        var clientId = configurationManager.GetSection("KeyVault").GetSection("ClientId").Value!;
        var clientSecret = configurationManager.GetSection("KeyVault").GetSection("ClientSecret").Value!;
        var directoryId = configurationManager.GetSection("KeyVault").GetSection("DirectoryId").Value!;

        var credential = new ClientSecretCredential(directoryId, clientId, clientSecret);
        configurationManager.AddAzureKeyVault(keyVaultUrl, credential);
        return services;
    }
}