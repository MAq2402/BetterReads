using BetterReads.Books.Application.Models;
using BetterReads.Books.Application.Services;
using MediatR;

namespace BetterReads.Books.Application.Queries;

public record SearchBooks(string SearchQuery) : IRequest<List<Book>>;

public class SearchBooksHandler(IBookSearchService bookSearchService) : IRequestHandler<SearchBooks, List<Book>>
{
    public async Task<List<Book>> Handle(SearchBooks request, CancellationToken cancellationToken)
    {
        return await bookSearchService.Search(request.SearchQuery);
    }
}