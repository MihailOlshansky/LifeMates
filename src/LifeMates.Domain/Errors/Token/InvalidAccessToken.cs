namespace LifeMates.Domain.Errors.Token;

public class InvalidAccessToken : ValidationError
{
    public InvalidAccessToken()
    {
        Message = "Некорректный токен доступа";
    }
}