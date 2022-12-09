using FluentResults;
using LifeMates.Domain.Models.Users;

namespace LifeMates.Domain.Models.Chats;

public class ChatUser
{
    public long Id { get; private set; }
    public long ChatId { get; private set; }
    public long UserId { get; private set; }
    public bool IsSeen { get; private set; }
    
    public Chat Chat { get; private set; }
    public User User { get; private set; }
    
    public ChatUser(long userId, bool isSeen)
    {
        UserId = userId;
        IsSeen = isSeen;
    }

    public Result<bool> Update(bool isSeen)
    {
        if (isSeen == IsSeen)
        {
            return Result.Ok(false);
        }
        
        IsSeen = isSeen;
        
        return Result.Ok(true);
    }
    
    protected ChatUser()
    {
        
    }
}