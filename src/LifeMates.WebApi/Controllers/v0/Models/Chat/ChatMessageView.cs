namespace LifeMates.WebApi.Controllers.v0.Models.Chat;

public class ChatMessageView
{
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserChatView User { get; set; }
    public bool IsSeen { get; set; }
}