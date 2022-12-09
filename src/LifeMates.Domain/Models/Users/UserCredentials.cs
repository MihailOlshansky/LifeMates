using Microsoft.AspNetCore.Identity;

namespace LifeMates.Domain.Models.Users;

public sealed class UserCredentials : IdentityUser
{
    public long UserId { get; private set; }

    public UserCredentials(long userId, string email)
    {
        UserId = userId;
        Email = email;
        UserName = email;
    }
    
    public UserCredentials(long userId, string email, string userName)
    {
        UserId = userId;
        Email = email;
        UserName = userName;
    }
    
    public UserCredentials()
    {

    }
}