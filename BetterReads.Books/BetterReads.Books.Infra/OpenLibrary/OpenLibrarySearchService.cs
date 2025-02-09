using BetterReads.Books.Application.Models;
using BetterReads.Books.Application.Services;
using BetterReads.Books.Infra.Caching;
using Microsoft.Extensions.Logging;

namespace BetterReads.Books.Infra.OpenLibrary;

public class OpenLibrarySearchService(
    IBooksCache booksCache,
    OpenLibraryHttpClient httpClient,
    ILogger<OpenLibrarySearchService> logger) : IBookSearchService
{
    public async Task<List<Book>> Search(string searchTerm)
    {
        var cachedBooks = await booksCache.Get(searchTerm);

        if (cachedBooks is not null)
            return cachedBooks;

        var apiResult = await httpClient.Search(searchTerm) ?? new SearchResult();

        var result = apiResult.Docs.Select(x =>
                new Book(Author: x.AuthorName.FirstOrDefault(), FirstPublishedYear: x.FirstPublishYear, Title: x.Title))
            .ToList();

        await booksCache.Set(searchTerm, result);
        return result;
    }
}