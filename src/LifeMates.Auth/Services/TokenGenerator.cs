using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LifeMates.Auth.Options;
using LifeMates.Auth.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LifeMates.Auth.Services;

public class TokenGenerator : ITokenGenerator
{
    private readonly IOptions<JwtOptions> _options;

    public TokenGenerator(IOptions<JwtOptions> options)
    {
        _options = options;
    }
    
    public string Generate(TokenType tokenType, IEnumerable<Claim>? claims = null)
    {
        var issuer = _options.Value.Issuer;
        var audience = _options.Value.Audience;
        var key = Encoding.ASCII.GetBytes(
            tokenType switch
            {
                TokenType.Access => _options.Value.AccessTokenSecret,
                TokenType.Refresh => _options.Value.RefreshTokenSecret,
                _ => throw new ArgumentOutOfRangeException(nameof(tokenType), tokenType, null)
            });
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = tokenType switch
            {
                TokenType.Access => DateTime.UtcNow.AddHours(_options.Value.TokenValidityInHours),
                TokenType.Refresh => DateTime.UtcNow.AddDays(_options.Value.RefreshTokenValidityInDays),
                _ => throw new ArgumentOutOfRangeException(nameof(tokenType), tokenType, null)
            },
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var stringToken = tokenHandler.WriteToken(token);

        return stringToken;
    }
}