using System.IdentityModel.Tokens.Jwt;
using BetterReads.Auth.Application.Contracts;
using BetterReads.Auth.Application.Dtos;
using BetterReads.Auth.Infra.Models;
using BetterReads.Auth.Infra.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BetterReads.Auth.Infra.Services;

public class CognitoService : IIdentityService
{
    private readonly HttpClient _httpClient;
    private readonly IdentityOptions _options;

    public CognitoService(HttpClient httpClient, IOptions<IdentityOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _httpClient.BaseAddress = new Uri(_options.Url);
    }
    
    public async Task<LoginResponse?> Login(string code)
    {
        var dict = new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "code", code },
            { "client_id", _options.ClientId },
            { "client_secret", _options.ClientSecret },
            { "redirect_uri", _options.RedirectUri }
        };
        var response = await _httpClient.PostAsync($"oauth2/token", new FormUrlEncodedContent(dict));
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var data = JsonConvert.DeserializeObject<CognitoTokenResponse>(await response.Content.ReadAsStringAsync());

        if (data is null)
        {
            return null;
        }
        
        var decodedAccessToken = new JwtSecurityTokenHandler().ReadJwtToken(data.access_token);
        var decodedIdToken = new JwtSecurityTokenHandler().ReadJwtToken(data.id_token);
        return new LoginResponse
        {
            Token = data.access_token,
            Email = decodedIdToken.Claims.First(c => c.Type == "email").Value,
            UserId = decodedAccessToken.Claims.First(c => c.Type == "sub").Value,
        };
    }
}