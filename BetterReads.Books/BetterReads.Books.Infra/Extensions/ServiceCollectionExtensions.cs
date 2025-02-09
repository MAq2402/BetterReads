using BetterReads.Books.Application.Services;
using BetterReads.Books.Infra.Caching;
using BetterReads.Books.Infra.OpenLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BetterReads.Books.Infra.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddScoped<IBooksCache, RedisBooksCache>();
        services.AddScoped<IBookSearchService, OpenLibrarySearchService>();
        services.AddHttpClient<OpenLibraryHttpClient>();
        services.AddStackExchangeRedisCache(option =>
        {
            option.Configuration = configuration["Redis:ConnectionString"];
        });
        services.Configure<OpenLibrarySettings>(configuration.GetSection("OpenLibrary"));
        return services;
    }
}