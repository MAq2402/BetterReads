﻿using BetterReads.Auth.Application.Dtos;

namespace BetterReads.Auth.Application.Contracts;

public interface IIdentityService
{
    Task<LoginResponse?> Login(string code);
    Task<Guid> Register(string email, string password);
}