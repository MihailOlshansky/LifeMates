using System.Text;
using LifeMates.Auth.Options;
using LifeMates.Auth.Services;
using LifeMates.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace LifeMates.Auth.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptionsSection = configuration.GetSection(JwtOptions.SectionName);
        services.Configure<JwtOptions>(jwtOptionsSection);
        var jwtOptions = jwtOptionsSection.Get<JwtOptions>();
        
        services.AddTransient<ITokenGenerator, TokenGenerator>();
        services.AddTransient<IRefreshTokenValidator, RefreshTokenValidator>();
        services.AddTransient<IAccessTokenDecoder, AccessTokenDecoder>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<ICurrentUser, CurrentUser>();
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(jwtOptions.AccessTokenSecret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });
        services.AddAuthorization();
            
        return services;
    }
}