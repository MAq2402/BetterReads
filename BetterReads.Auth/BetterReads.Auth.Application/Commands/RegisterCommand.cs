using BetterReads.Auth.Application.Contracts;
using BetterReads.Shared.Application.Events;
using BetterReads.Shared.Application.Services;
using MediatR;

namespace BetterReads.Auth.Application.Commands;

public record RegisterCommand(string Email, string Password) : IRequest;

public class RegisterCommandHandler(IIdentityService identityService, IIntegrationEventPublisher integrationEventPublisher) : IRequestHandler<RegisterCommand>
{
    public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await identityService.Register(request.Email, request.Password);
        await integrationEventPublisher.Publish(new UserRegistered(result));
    }
}