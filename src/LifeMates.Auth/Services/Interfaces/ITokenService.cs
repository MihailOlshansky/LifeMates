using LifeMates.Domain.Models.Users;
using LifeMates.Domain.Shared.Users;

namespace LifeMates.Auth.Services.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(UserCredentials user);
    string GenerateAccessToken(UserCredentials user, RoleTypes roleType);
    string GenerateRefreshToken();
}