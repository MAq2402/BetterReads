using BetterReads.Books.Application.Models;

namespace BetterReads.Books.Infra.Caching;

public interface IBooksCache
{
    Task<List<Book>?> Get(string key);
    Task Set(string key, List<Book> value);
}