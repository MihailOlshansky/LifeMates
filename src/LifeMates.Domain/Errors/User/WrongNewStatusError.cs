namespace LifeMates.Domain.Errors.User;

public class WrongNewStatusError : ApplicationError
{
    public WrongNewStatusError()
    {
        Message = "Неверный новый статус";
    }
}