using LifeMates.Domain.Models.Users;

namespace LifeMates.Domain.Models.Chats;

public class RequestHistory
{
    public long Id { get; private set; }
    public long UserId { get; private set; }
    public DateTime Time { get; private set; }
    
    public User User { get; private set; }

    public RequestHistory(long userId)
    {
        UserId = userId;
        Time = DateTime.Now;
    }

    public void Update()
    {
        Time = DateTime.Now;
    }
}