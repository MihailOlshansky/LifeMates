using LifeMates.Domain.Models.Users;

namespace LifeMates.Storage.SharedKernel.Repositories;

public interface IUserRepository
{
    Task Commit(CancellationToken cancellationToken);
    
    Task Create(User user, CancellationToken cancellationToken);

    Task<User?> Get(long id, CancellationToken cancellationToken);

    Task<bool> Exists(long id, CancellationToken cancellationToken);
}