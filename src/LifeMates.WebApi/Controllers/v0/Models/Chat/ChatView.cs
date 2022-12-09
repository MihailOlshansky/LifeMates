namespace LifeMates.WebApi.Controllers.v0.Models.Chat;

public class ChatView
{
    public long Id { get; set; }
    public UserChatView User { get; set; }
    public ChatMessageView Message { get; set; }
}