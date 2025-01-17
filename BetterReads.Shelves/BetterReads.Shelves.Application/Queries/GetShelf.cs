using BetterReads.Shared.Application.Exceptions;
using BetterReads.Shelves.Application.Dtos;
using BetterReads.Shelves.Application.Mappings;
using BetterReads.Shelves.Domain;
using BetterReads.Shelves.Domain.Repositories;
using MediatR;

namespace BetterReads.Shelves.Application.Queries;

public record GetShelf(Guid UserId, Guid ShelfId) : IRequest<ShelfDto>;
public class GetShelfHandler(IShelvesRepository shelvesRepository) : IRequestHandler<GetShelf, ShelfDto>
{
    public async Task<ShelfDto> Handle(GetShelf request, CancellationToken cancellationToken)
    {
        var shelf = await shelvesRepository.Get(request.ShelfId, request.UserId);

        if (shelf == null)
        {
            throw new AggregateNotFoundException(nameof(Shelf), request.ShelfId);
        }

        return shelf.AsDto();
    }
}