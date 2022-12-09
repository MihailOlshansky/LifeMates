using FluentResults;
using LifeMates.Domain.Models.Users;

namespace LifeMates.Domain.Models.Chats;

public class Message
{
    public long Id { get; private set; }
    public long ChatId { get; private set; }
    public long UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string Content { get; private set; }
    public bool IsSeen { get; private set; }
    
    public User User { get; private set; }
    public Chat Chat { get; private set; }

    public Result<bool> Update(bool isSeen)
    {
        if (IsSeen == isSeen)
        {
            return Result.Ok(false);
        }

        IsSeen = isSeen;

        return Result.Ok(true);
    }

    public Message(long chatId, long userId, string content)
    {
        ChatId = chatId;
        UserId = userId;
        Content = content;
        IsSeen = false;
        CreatedAt = DateTime.Now;
    }
}