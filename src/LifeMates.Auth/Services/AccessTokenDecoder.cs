using System.IdentityModel.Tokens.Jwt;
using System.Text;
using LifeMates.Auth.Constants;
using LifeMates.Auth.Options;
using LifeMates.Auth.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LifeMates.Auth.Services;

public class AccessTokenDecoder : IAccessTokenDecoder
{
    private readonly TokenValidationParameters _validationParameters;

    public AccessTokenDecoder(IOptions<JwtOptions> options)
    {
        _validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.AccessTokenSecret)),
            ValidIssuer = options.Value.Issuer,
            ValidAudience = options.Value.Audience,
            ClockSkew = TimeSpan.Zero
        };
    }
    
    public string? GetUserCredentialsId(string accessToken)
    {
        var claims = new JwtSecurityTokenHandler().ValidateToken(accessToken, _validationParameters, out _);
        var userIdClaimValue = claims.FindFirst(AuthConstants.UserClaims.UserCredentialsId)?.Value;

        return userIdClaimValue;
    }

    public string? GetUserId(string accessToken)
    {
        var claims = new JwtSecurityTokenHandler().ValidateToken(accessToken, _validationParameters, out _);
        var userIdClaimValue = claims.FindFirst(AuthConstants.UserClaims.UserId)?.Value;

        return userIdClaimValue;
    }

    public string? GetUserRoleType(string accessToken)
    {
        var claims = new JwtSecurityTokenHandler().ValidateToken(accessToken, _validationParameters, out _);
        var userIdClaimValue = claims.FindFirst(AuthConstants.UserClaims.RoleType)?.Value;

        return userIdClaimValue;
    }
}