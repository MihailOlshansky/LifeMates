using LifeMates.Domain.Shared.Users;

namespace LifeMates.Domain.Models.Users;

public class UserContact
{
    public long Id { get; private set; }
    public long UserId { get; private set; }
    public ContactType Type { get; private set; }
    public string Value { get; private set; }

    public User? User { get; private set; }
    
    public UserContact(long id, long userId, ContactType type, string value)
    {
        Id = id;
        UserId = userId;
        Type = type;
        Value = value;
    }
    
    public UserContact(ContactType type, string value) : this(default, default, type, value) {}

    protected UserContact()
    {
        
    }
}