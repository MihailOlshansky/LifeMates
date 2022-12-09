namespace LifeMates.Domain.Errors.Token;

public class RevokeRefreshTokenError : ApplicationError
{
    public RevokeRefreshTokenError()
    {
        Message = "Ошибка при отзыве токена обновления доступа";
    }
}