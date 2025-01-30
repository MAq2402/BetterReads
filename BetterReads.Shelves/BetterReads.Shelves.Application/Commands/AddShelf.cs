using BetterReads.Shelves.Domain;
using BetterReads.Shelves.Domain.Exceptions;
using BetterReads.Shelves.Domain.Repositories;
using MediatR;

namespace BetterReads.Shelves.Application.Commands;

public record AddShelf(string Name, Guid UserId) : IRequest;

public class AddShelfHandler(IShelvesRepository repository) : IRequestHandler<AddShelf>
{
    public async Task Handle(AddShelf request, CancellationToken cancellationToken)
    {
        var shelves = await repository.GetMany(request.UserId);

        if (shelves.Any(x => x.Name == request.Name))
        {
            throw new ShelfWithNameAlreadyExistsException("Shelf with name already exists");
        }
        
        await repository.AddShelf(new Shelf(Guid.NewGuid(), request.Name, request.UserId));
    }
}