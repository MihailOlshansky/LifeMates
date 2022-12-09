namespace LifeMates.Domain.Models.Chats;

public class Chat
{
    public long Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public ICollection<ChatUser> ChatUsers { get; private set; }
    public ICollection<Message> Messages { get; private set; }

    public Chat(ChatUser userIdA, ChatUser userIdB)
    {
        ChatUsers = new List<ChatUser>() { userIdA, userIdB };
        CreatedAt = DateTime.UtcNow;
    }

    public bool IsUserChat(long userId)
    {
        return ChatUsers.Any(x => x.UserId == userId);
    }
    
    protected Chat()
    {
        
    }

    public void AddMessage(long currentUserId, string requestMessage)
    {
        Messages ??= new List<Message>();
        
        Messages.Add(new Message(Id, currentUserId, requestMessage));
    }
}