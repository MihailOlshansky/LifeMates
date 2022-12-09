using LifeMates.Domain.Models.Users;
using LifeMates.Storage.SharedKernel.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeMates.Storage.Repositories;

public class UserDislikeRepository : IUserDislikeRepository
{
    private readonly LifematesDbContext _dbContext;

    public UserDislikeRepository(LifematesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Create(UserDislikes dislikes)
    {
        _dbContext.UserDislikes.Add(dislikes);
    }

    public Task<UserDislikes?> Get(long userId, long dislikedUserId, CancellationToken cancellationToken)
    {
        return _dbContext.UserDislikes.FirstOrDefaultAsync(
            x => x.UserId == userId && x.DislikedUserId == dislikedUserId,
            cancellationToken);
    }
    
    public Task<bool> Exist(long userId, long dislikedUserId, CancellationToken cancellationToken)
    {
        return _dbContext.UserDislikes.AnyAsync(
            x => x.UserId == userId && x.DislikedUserId == dislikedUserId,
            cancellationToken: cancellationToken);
    }
}