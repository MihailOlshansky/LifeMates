namespace LifeMates.Domain.Errors.User;

public class UserWithEmailExistsError : ApplicationError
{
    public UserWithEmailExistsError()
    {
        Message = "Уже есть пользователь зарегистрированный с данной почтой. Забыли пароль?";
    }
}