using BetterReads.Auth.Application.Contracts;
using BetterReads.Auth.Infra.Options;
using BetterReads.Auth.Infra.Services;
using BetterReads.Shared.Infra.Extensions;
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
        return services;
    }
}