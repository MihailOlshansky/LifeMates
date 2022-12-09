namespace LifeMates.Domain.ReadOnly.Chats;

public class UserChatView
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<string>? ImagesUrls { get; set; }
}