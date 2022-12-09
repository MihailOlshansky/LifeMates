namespace LifeMates.WebApi.Controllers.v0.Models.Chat.GetMetches;

public class GetMatchesResponse
{
    public ICollection<MatchView> Matches { get; set; } = new List<MatchView>();
    
    public int Count { get; set; }
}