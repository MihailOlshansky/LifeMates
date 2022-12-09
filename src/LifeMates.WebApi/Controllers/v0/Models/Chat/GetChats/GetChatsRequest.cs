namespace LifeMates.WebApi.Controllers.v0.Models.Chat.GetChats;

public class GetChatsRequest : IPagination
{
    public int Offset { get; set; }
    public int Limit { get; set; }
}