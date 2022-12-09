namespace LifeMates.Domain.Errors.Token;

public class InvalidRefreshToken : ValidationError
{
    public InvalidRefreshToken()
    {
        Message = "Некорректный токен обновления доступа";
    }
}