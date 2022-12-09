namespace LifeMates.WebApi.Controllers.v0.Models.Chat.GetMessages;

public class GetMessagesResponse
{
    public ICollection<MessageView> Messages { get; set; } = default!;
    public int Count { get; set; }
}