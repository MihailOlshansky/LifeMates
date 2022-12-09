namespace LifeMates.Domain.Errors.User;

public class UserSettingsNotFoundError : ApplicationError
{
    public UserSettingsNotFoundError()
    {
        Message = "Не найдены настройки пользователя";
    }
}