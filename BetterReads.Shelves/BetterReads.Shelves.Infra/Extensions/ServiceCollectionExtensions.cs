using BetterReads.Shared.Infra.Extensions;
using BetterReads.Shelves.Application.Consumers;
using BetterReads.Shelves.Application.Repositories;
using BetterReads.Shelves.Application.Sagas;
using BetterReads.Shelves.Domain.Repositories;
using BetterReads.Shelves.Infra.Mongo.Repositories;
using MassTransit;
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
        services.AddMongoOutbox();
        services.AddSingleton<ITransactionShelvesRepository, MongoShelvesRepository>();
        services.AddSingleton<IShelvesRepository, MongoShelvesRepository>();
        services.AddMassTransitPublisher();
        services.AddTelemetry("Shelves");
        
        services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(CreateDefaultShelvesConsumer));
            x.AddSagaStateMachine<NewUserSaga, NewUserInstance>()
                .MongoDbRepository(r =>
                {
                    r.Connection = configuration.GetSection("Mongo").GetValue<string>("ConnectionString");
                    r.DatabaseName = configuration.GetSection("Mongo").GetValue<string>("DatabaseName");
                    r.CollectionName = "NewUserSagas";
                });
            x.UsingAzureServiceBus((context,cfg) =>
            {
                cfg.UseInstrumentation();
                cfg.Host(configuration.GetSection("AzureServiceBus").GetValue<string>("ConnectionString"));
                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}