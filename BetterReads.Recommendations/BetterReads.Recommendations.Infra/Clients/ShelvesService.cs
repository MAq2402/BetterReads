using System.Text.Json;
using BetterReads.Recommendations.Application.Models;
using BetterReads.Recommendations.Application.Services;
using BetterReads.Recommendations.Infra.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BetterReads.Recommendations.Infra.Clients;

public class ShelvesService : IShelvesService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ShelvesService> _logger;

    public ShelvesService(HttpClient httpClient, IOptions<ShelvesClientSettings> options,
        ILogger<ShelvesService> logger)
    {
        _logger = logger;
        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri(options.Value.Url);
    }

    public async Task<List<ShelfDto>> GetShelves(Guid userId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"users/{userId}/shelves");
        var result = await _httpClient.SendAsync(request);

        if (!result.IsSuccessStatusCode)
        {
            _logger.LogError($"GetShelves failed with status code {result.StatusCode}");
            return new List<ShelfDto>();
        }

        _logger.LogInformation($"GetShelves succeeded with status code {result.StatusCode}");
        _logger.LogDebug($"GetShelves message: {await result.Content.ReadAsStringAsync()}");

        return JsonSerializer.Deserialize<List<ShelfDto>>(await result.Content.ReadAsStringAsync()) ??
               new List<ShelfDto>();
    }
}