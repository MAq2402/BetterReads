using BetterReads.Shared.Application.Events;
using BetterReads.Shared.Application.Exceptions;
using BetterReads.Shared.Application.Services;
using BetterReads.Shelves.Domain;
using BetterReads.Shelves.Domain.Repositories;
using MediatR;

namespace BetterReads.Shelves.Application.Commands;

public record AddBook(
    string Name,
    string Author,
    string Isbn,
    string Language,
    int YearOfPublication,
    Guid UserId,
    Guid ShelfId) : IRequest;

public class AddBookHandler(IShelvesRepository shelvesRepository, IIntegrationEventPublisher publisher) : IRequestHandler<AddBook>
{
    public async Task Handle(AddBook request, CancellationToken cancellationToken)
    {
        var shelf = await shelvesRepository.Get(request.ShelfId, request.UserId);

        if (shelf is null)
        {
            throw new AggregateNotFoundException(nameof(Shelf), request.ShelfId);
        }
        
        shelf.AddBook(new Book(request.Name, request.Author, request.Isbn, request.Language, request.YearOfPublication));
        await shelvesRepository.Save(shelf);

        await publisher.Publish(new BookAdded(shelf.UserId));
    }
}