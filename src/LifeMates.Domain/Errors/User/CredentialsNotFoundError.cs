namespace LifeMates.Domain.Errors.User;

public class CredentialsNotFoundError : ApplicationError
{
    public CredentialsNotFoundError()
    {
        Message = "Не найдены учётные данные пользователя";
    }
}