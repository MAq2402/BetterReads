using BetterReads.Shelves.Application.Dtos;
using BetterReads.Shelves.Application.Mappings;
using BetterReads.Shelves.Domain.Repositories;
using MediatR;

namespace BetterReads.Shelves.Application.Queries;
public record GetShelves(Guid UserId) : IRequest<List<ShelfDto>>;
public class GetShelvesHandler(IShelvesRepository shelvesRepository) : IRequestHandler<GetShelves, List<ShelfDto>>
{
    public async Task<List<ShelfDto>> Handle(GetShelves request, CancellationToken cancellationToken)
    {
        return (await shelvesRepository.GetMany(request.UserId)).Select(x => x.AsDto()).ToList();
    }
}