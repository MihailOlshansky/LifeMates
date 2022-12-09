using LifeMates.Domain.Models.Users;

namespace LifeMates.Storage.SharedKernel.Repositories;

public interface IUserLikesRepository
{
    void Create(UserLikes like);
    Task<bool> Exist(long userId, long likedUserId, CancellationToken cancellationToken);
}