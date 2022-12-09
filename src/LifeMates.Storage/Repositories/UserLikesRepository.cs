using LifeMates.Domain.Models.Users;
using LifeMates.Storage.SharedKernel.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeMates.Storage.Repositories;

public class UserLikesRepository : IUserLikesRepository
{
    private readonly LifematesDbContext _dbContext;

    public UserLikesRepository(LifematesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Create(UserLikes like)
    {
        _dbContext.UsersLikes.Add(like);
    }

    public Task<UserLikes?> Get(long userId, long likedUserId, CancellationToken cancellationToken)
    {
        return _dbContext.UsersLikes.FirstOrDefaultAsync(
            x => x.UserId == userId && x.LikedUserId == likedUserId,
            cancellationToken);
    }

    public Task<bool> Exist(long userId, long likedUserId, CancellationToken cancellationToken)
    {
        return _dbContext.UsersLikes.AnyAsync(
            x => x.UserId == userId && x.LikedUserId == likedUserId,
            cancellationToken: cancellationToken);
    }
}