using BetterReads.Shared.Infra.Extensions;
using BetterReads.Shelves.Domain.Repositories;
using BetterReads.Shelves.Infra.Mongo.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BetterReads.Shelves.Infra.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddKeyVault(configuration);
        services.AddCognitoJwtAuth(configuration);
        services.AddMongo(configuration);
        services.AddScoped<IShelvesRepository, MongoShelvesRepository>();
        return services;
    }
}