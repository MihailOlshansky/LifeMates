using System.IdentityModel.Tokens.Jwt;
using System.Text;
using LifeMates.Auth.Options;
using LifeMates.Auth.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LifeMates.Auth.Services;

public class RefreshTokenValidator : IRefreshTokenValidator
{
    private readonly IOptions<JwtOptions> _options;

    public RefreshTokenValidator(IOptions<JwtOptions> options)
    {
        _options = options;
    }
    
    public bool Validate(string refreshToken)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.RefreshTokenSecret)),
            ValidIssuer = _options.Value.Issuer,
            ValidAudience = _options.Value.Audience,
            ClockSkew = TimeSpan.Zero
        };

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        try
        {
            jwtSecurityTokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}