namespace LifeMates.Domain.Errors.User;

public class UserNotFoundError : ApplicationError
{
    public UserNotFoundError()
    {
        Message = "Пользователь не найден";
    }
    
    public UserNotFoundError(long userId)
    {
        Message = $"Пользователь (id = {userId}) не найден";
        PropertyName = nameof(Models.Users.User.Id);
    }
}