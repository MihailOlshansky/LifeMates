using FluentResults;

namespace LifeMates.Core.Services;

public interface IAuthService
{
    Task<Result<(string AccessToken, string RefreshToken)>> GenerateTokens(
        string email, 
        string password,
        CancellationToken cancellationToken);
}