using LifeMates.Domain.Models.Users;

namespace LifeMates.Storage.SharedKernel.Repositories;

public interface IUserDislikeRepository
{
    void Create(UserDislikes dislikes);
    Task<UserDislikes?> Get(long userId, long dislikedUserId, CancellationToken cancellationToken);
}