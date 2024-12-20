using BetterReads.Auth.Application.Contracts;
using BetterReads.Auth.Application.Dtos;
using MediatR;
namespace BetterReads.Auth.Application.Commands;

public record LoginQuery(string Code) : IRequest<LoginResponse>;

public class LoginCommandHandler(IIdentityService identityService) : IRequestHandler<LoginQuery, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var result = await identityService.Login(request.Code);

        if (result is null)
        {
            throw new ApplicationException("Failed to login with given code.");
        }

        return result;
    }
}