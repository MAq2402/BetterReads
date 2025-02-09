using System.Net.Http.Json;
using Microsoft.Extensions.Options;

namespace BetterReads.Books.Infra.OpenLibrary;

public class OpenLibraryHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly OpenLibrarySettings _openLibrarySettings;

    public OpenLibraryHttpClient(HttpClient httpClient, IOptions<OpenLibrarySettings> options)
    {
        _httpClient = httpClient;
        _openLibrarySettings = options.Value;
        _httpClient.BaseAddress = new Uri(_openLibrarySettings.Url);
    }

    public async Task<SearchResult?> Search(string searchTerm) =>
        await _httpClient.GetFromJsonAsync<SearchResult>(
            $"search.json?q={searchTerm}&limit={_openLibrarySettings.Limit ?? 10}");
}