namespace LifeMates.WebApi.Controllers.v0.Models.Chat.GetChats;

public class GetChatsResponse
{
    public ICollection<ChatView> Chats { get; set; } = new List<ChatView>();
    
    public int Count { get; set; }
}