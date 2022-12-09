using LifeMates.Domain.ReadOnly.Users;

namespace LifeMates.Domain.ReadOnly.Chats;

public class MatchView
{
    public long Id { get; set; }
    public UserView User { get; set; }
}