using BetterReads.Books.Application.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BetterReads.Books.Infra.Caching;

public class RedisBooksCache(IDistributedCache distributedCache, ILogger<RedisBooksCache> logger) : IBooksCache
{
    public async Task<List<Book>?> Get(string key)
    {
        try
        {
            var result = await distributedCache.GetStringAsync(key);

            if (string.IsNullOrEmpty(result))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<List<Book>>(result);
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to get books from cache. Exception: {ex}", ex);
            return null;
        }
    }

    public async Task Set(string key, List<Book> value)
    {
        try
        {
            await distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value));
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to set books in the cache. Exception: {ex}", ex);
        }
    }
}