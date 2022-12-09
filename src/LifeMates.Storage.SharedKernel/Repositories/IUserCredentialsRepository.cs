using LifeMates.Domain.Models.Users;

namespace LifeMates.Storage.SharedKernel.Repositories;

public interface IUserCredentialsRepository
{
    Task<bool> Exists(string email, CancellationToken cancellationToken);
    Task<UserCredentials?> FindByEmail(string email, CancellationToken cancellationToken);
    Task<UserCredentials> FindById(string id, CancellationToken cancellationToken);

    Task<bool> CheckPasswordSignIn(UserCredentials userCredentials, string password, CancellationToken cancellationToken);

    Task SetRefreshToken(UserCredentials userCredentials, string refreshToken, CancellationToken cancellationToken);
    Task<bool> RemoveRefreshToken(UserCredentials userCredentials, CancellationToken cancellationToken);
    Task<string> GetRefreshToken(UserCredentials userCredentials, CancellationToken cancellationToken);

    Task<bool> Create(UserCredentials userCredentials, string password, CancellationToken cancellationToken);
    Task<bool> IsInRole(UserCredentials userCredentials, string role, CancellationToken cancellationToken);
}