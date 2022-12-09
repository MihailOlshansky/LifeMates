namespace LifeMates.Domain.Models.Users;

public class UserImage
{
    public long Id { get; private set; }
    public long UserId { get; private set; }
    public string Url { get; private set; }

    public User? User { get; private set; }
    
    public UserImage(long id, long userId, string url)
    {
        Id = id;
        UserId = userId;
        Url = url;
    }
    
    public UserImage(string url) : this(default, default, url) {}

    protected UserImage()
    {
        
    }
}
