using BetterReads.Shared.Application.Exceptions;
using BetterReads.Shared.Application.Repositories;
using BetterReads.Shelves.Application.Repositories;
using BetterReads.Shelves.Domain;
using MediatR;
using BookAdded = BetterReads.Shared.Application.Events.BookAdded;

namespace BetterReads.Shelves.Application.Commands;

public record AddBook(
    string Name,
    string Author,
    string Isbn,
    string Language,
    int YearOfPublication,
    Guid UserId,
    Guid ShelfId) : IRequest;

public class AddBookHandler(ITransactionShelvesRepository shelvesRepository, IOutboxRepository outboxRepository, IUnitOfWork unitOfWork) : IRequestHandler<AddBook>
{
    public async Task Handle(AddBook request, CancellationToken cancellationToken)
    {
        var shelf = await shelvesRepository.Get(request.ShelfId, request.UserId);

        if (shelf is null)
        {
            throw new AggregateNotFoundException(nameof(Shelf), request.ShelfId);
        }
        
        shelf.AddBook(new Book(request.Name, request.Author, request.Isbn, request.Language, request.YearOfPublication));
        
        await unitOfWork.Transaction(async session =>
        {
            await shelvesRepository.Save(shelf, session);
            await outboxRepository.Add(new BookAdded(shelf.UserId), session);
        });
    }
}