using BetterReads.Shared.Application.Dtos;
using BetterReads.Shared.Application.Exceptions;
using BetterReads.Shelves.Application.Commands;
using MediatR;

namespace BetterReads.Shelves.Application.Dtos;

public class CreateShelfDto : ModifyDto
{
    public string Name { get; set; }
    
    public IRequest ToCommand(Guid? userId = null)
    {
        if (!userId.HasValue)
        {
            throw new UnauthorizedException(nameof(userId));
        }

        return new AddShelf(Name, userId.Value);
    }
}