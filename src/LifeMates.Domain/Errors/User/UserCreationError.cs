namespace LifeMates.Domain.Errors.User;

public class UserCreationError : ApplicationError
{
    public UserCreationError()
    {
        Message = "Не получается создать пользователя";
    }
}