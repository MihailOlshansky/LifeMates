using FluentResults;

namespace LifeMates.Domain.Models.Users;

public class UserDislikes
{
    public long Id { get; private set; }
    public long UserId { get; private set; }
    public long DislikedUserId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    public User User { get; private set; }
    
    public UserDislikes(long id, long userId, long dislikedUserId)
    {
        Id = id;
        UserId = userId;
        DislikedUserId = dislikedUserId;
        CreatedAt = DateTime.UtcNow;
        ModifiedAt = DateTime.UtcNow;
    }
    
    public UserDislikes(long userId, long likedUserId) : this(default, userId, likedUserId) {}

    public Result Update()
    {
        ModifiedAt = DateTime.UtcNow;
        
        return Result.Ok();
    }
    
    protected UserDislikes() {}
}