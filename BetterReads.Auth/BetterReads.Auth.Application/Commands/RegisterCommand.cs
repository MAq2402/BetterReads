using BetterReads.Auth.Application.Contracts;
using MediatR;

namespace BetterReads.Auth.Application.Commands;

public record RegisterCommand(string Email, string Password) : IRequest;

public class RegisterCommandHandler(IIdentityService identityService) : IRequestHandler<RegisterCommand>
{
    public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        await identityService.Register(request.Email, request.Password);
    }
}