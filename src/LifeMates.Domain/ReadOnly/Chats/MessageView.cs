namespace LifeMates.Domain.ReadOnly.Chats;

public class MessageView
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsSeen { get; set; }
}