using MediatR;

namespace BetterReads.Shared.Application.Dtos;

public interface ModifyDto
{
    public IRequest ToCommand(Guid? userId = null);
}