using FluentResults;
using LifeMates.Domain.Shared.Users;

namespace LifeMates.Domain.Models.Users;

public class UserSettings
{
    public long Id { get; private set; }
    public long UserId { get; private set; }
    public UserGender ShowingGender { get; private set; }

    public User User { get; private set; }
    
    public UserSettings(UserGender showingGender)
    {
        ShowingGender = showingGender;
    }
    
    public UserSettings(long id, long userId, UserGender showingGender)
    {
        Id = id;
        UserId = userId;
        ShowingGender = showingGender;
    }

    public Result<bool> Update(UserSettings settings)
    {
        if (ShowingGender == settings.ShowingGender)
        {
            return Result.Ok(false);
        }

        ShowingGender = settings.ShowingGender;

        return Result.Ok(true);
    }
    
    protected UserSettings()
    {
        
    }
}