namespace LifeMates.WebApi.Controllers.v0.Models.Chat.GetMessages;

public class GetMessagesRequest : IPagination
{
    public int Offset { get; set; }
    public int Limit { get; set; }
}