using BetterReads.Auth.Application.Contracts;
using BetterReads.Auth.Infra.Options;
using BetterReads.Auth.Infra.Services;
using BetterReads.Shared.Infra.Extensions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BetterReads.Auth.Infra.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.Configure<IdentityOptions>(configuration.GetSection("IdentityService"));
        services.AddKeyVault(configuration);
        services.AddScoped<IIdentityService, CognitoService>();
        services.AddHttpClient<CognitoService>();
        services.AddMassTransitPublisher();
        services.AddTelemetry("Auth");
        
        services.AddMassTransit(x =>
        {
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