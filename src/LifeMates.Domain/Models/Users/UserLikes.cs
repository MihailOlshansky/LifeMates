namespace LifeMates.Domain.Models.Users;

public class UserLikes
{
    public long Id { get; private set; }
    public long UserId { get; private set; }
    public long LikedUserId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User User { get; private set; }
    
    public UserLikes(long id, long userId, long likedUserId, DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        LikedUserId = likedUserId;
        CreatedAt = createdAt;
    }
    
    public UserLikes(long userId, long likedUserId) : this(default, userId, likedUserId, DateTime.UtcNow) {}
    
    protected UserLikes() {}
}