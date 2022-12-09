namespace LifeMates.Domain.Errors.Interest;

public class InterestNotFoundError : ApplicationError
{
    public InterestNotFoundError()
    {
        Message = "Интерес не найден";
    }
}