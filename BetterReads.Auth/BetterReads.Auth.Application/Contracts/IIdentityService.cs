using BetterReads.Auth.Application.Dtos;

namespace BetterReads.Auth.Application.Contracts;

public interface IIdentityService
{
    Task<LoginResponse?> Login(string code);
    Task Register(string email, string password);
}