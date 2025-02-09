using BetterReads.Books.Application.Models;

namespace BetterReads.Books.Application.Services;

public interface IBookSearchService
{
    Task<List<Book>> Search(string searchTerm);
}