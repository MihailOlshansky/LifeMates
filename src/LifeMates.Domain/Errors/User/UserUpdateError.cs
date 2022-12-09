namespace LifeMates.Domain.Errors.User;

public class UserUpdateError : ApplicationError
{
    public UserUpdateError()
    {
        Message = "Ошибка при обновлении пользователя";
    }
}