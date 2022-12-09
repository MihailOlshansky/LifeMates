using LifeMates.Domain.Models.Users;

namespace LifeMates.Domain.Models.Interests;

public class Interest
{
    public long Id { get; private set; }
    public string Value { get; private set; }

    public ICollection<UserInterest>? Users { get; private set; }
    
    public Interest(long id, string value)
    {
        Id = id;
        Value = value;
    }
    
    public Interest(string value)
    {
        Value = value;
    }

    protected Interest()
    {
        
    }
}