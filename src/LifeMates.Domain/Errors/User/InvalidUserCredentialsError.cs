namespace LifeMates.Domain.Errors.User;

public class InvalidUserCredentialsError : ValidationError
{
    public InvalidUserCredentialsError()
    {
        Message = "Неверный адрес электронной почты или пароль";
    }
}