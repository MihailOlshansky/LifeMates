using LifeMates.Domain.Models.Interests;

namespace LifeMates.Domain.Models.Users;

public class UserInterest
{
    public long Id { get; private set; }
    public long UserId { get; private set; }
    public long InterestId { get; private set; }

    public User? User { get; private set; }
    public Interest? Interest { get; private set; }

    public UserInterest(long id, long userId, long interestId)
    {
        Id = id;
        UserId = userId;
        InterestId = interestId;
    }

    public UserInterest(long interestId) : this(default, default, interestId) {}
    
    protected UserInterest()
    {
        
    }
}