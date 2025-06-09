using BetterReads.Auth.Application.Contracts;
using BetterReads.Auth.Application.Dtos;
using BetterReads.Shared.Application.Exceptions;
using MediatR;
namespace BetterReads.Auth.Application.Queries;

public record LoginQuery(string Code) : IRequest<LoginResponse>;

public class LoginCommandHandler(IIdentityService identityService) : IRequestHandler<LoginQuery, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var result = await identityService.Login(request.Code);

        if (result is null)
        {
            throw new UnauthorizedException("Failed to login with given code.");
        }

        return result;
    }
}