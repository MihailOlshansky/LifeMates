using System.Security.Claims;

namespace LifeMates.Auth.Services.Interfaces;

public interface ITokenGenerator
{
    string Generate(TokenType tokenType, IEnumerable<Claim>? claims = null);
}