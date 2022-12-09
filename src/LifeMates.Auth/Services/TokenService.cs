using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LifeMates.Auth.Constants;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Domain.Models.Users;
using LifeMates.Domain.Shared.Users;

namespace LifeMates.Auth.Services;

public class TokenService : ITokenService
{
    private readonly ITokenGenerator _tokenGenerator;

    public TokenService(ITokenGenerator tokenGenerator)
    {
        _tokenGenerator = tokenGenerator;
    }

    public string GenerateAccessToken(UserCredentials user)
    {
        var claims = new[]
        {
            new Claim("Id", Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(AuthConstants.UserClaims.UserCredentialsId, user.Id),
            new Claim(AuthConstants.UserClaims.UserId, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        return _tokenGenerator.Generate(TokenType.Access, claims);
    }

    public string GenerateAccessToken(UserCredentials user, RoleTypes roleType)
    {
        var claims = new[]
        {
            new Claim("Id", Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(AuthConstants.UserClaims.UserCredentialsId, user.Id),
            new Claim(AuthConstants.UserClaims.UserId, user.UserId.ToString()),
            new Claim(AuthConstants.UserClaims.RoleType, roleType.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        return _tokenGenerator.Generate(TokenType.Access, claims);
    }

    public string GenerateRefreshToken()
    {
        return _tokenGenerator.Generate(TokenType.Refresh);
    }
}