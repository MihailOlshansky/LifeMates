namespace LifeMates.Domain.Errors.Match;

public class UpdateDislikeError : ApplicationError
{
    public UpdateDislikeError()
    {
        Message = "Ошибка при обновлении дизлайка";
    }
}