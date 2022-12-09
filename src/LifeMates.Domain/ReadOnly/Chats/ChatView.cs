namespace LifeMates.Domain.ReadOnly.Chats;

public class ChatView
{
    public long Id { get; set; }
    public UserChatView User { get; set; }
    public ChatMessageView Message { get; set; }
    
}