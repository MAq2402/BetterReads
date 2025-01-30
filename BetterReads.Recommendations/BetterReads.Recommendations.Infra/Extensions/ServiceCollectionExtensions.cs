using BetterReads.Recommendations.Domain.Repositories;
using BetterReads.Recommendations.Infra.Mongo.Repositories;
using BetterReads.Shared.Infra.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BetterReads.Recommendations.Infra.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddKeyVault(configuration);
        services.AddCognitoJwtAuth(configuration);
        services.AddMongo(configuration);
        services.AddSingleton<IRecommendationsRepository, MongoRecommendationsRepository>();
        return services;
    }
}