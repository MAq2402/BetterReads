using BetterReads.Recommendations.Application.Consumers;
using BetterReads.Recommendations.Domain.Repositories;
using BetterReads.Recommendations.Infra.Mongo.Repositories;
using BetterReads.Shared.Application.Commands;
using BetterReads.Shared.Application.Events;
using BetterReads.Shared.Infra.Extensions;
using MassTransit;
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
        services.AddTelemetry("Recommendations");

        services.AddMassTransit(x =>
        {
            x.AddConsumer<BookAddedConsumer>();
            x.AddConsumer<AddInitialRecommendationsConsumer>();
            x.SetKebabCaseEndpointNameFormatter();
            x.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.UseInstrumentation();
                cfg.Host(configuration.GetSection("AzureServiceBus").GetValue<string>("ConnectionString"));

                cfg.ReceiveEndpoint(
                    KebabCaseEndpointNameFormatter.Instance.SanitizeName(nameof(AddInitialRecommendations)),
                    e => e.ConfigureConsumer<AddInitialRecommendationsConsumer>(context));
                cfg.ReceiveEndpoint(KebabCaseEndpointNameFormatter.Instance.SanitizeName(nameof(BookAdded)),
                    e => e.ConfigureConsumer<BookAddedConsumer>(context));
            });
        });
        return services;
    }
}