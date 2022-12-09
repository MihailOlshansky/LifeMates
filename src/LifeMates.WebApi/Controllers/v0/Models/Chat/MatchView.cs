using LifeMates.WebApi.Controllers.v0.Models.User;

namespace LifeMates.WebApi.Controllers.v0.Models.Chat;

public class MatchView
{
    public long Id { get; set; }
    public bool IsSeen { get; set; }
    public UserView User { get; set; } = default!;
}