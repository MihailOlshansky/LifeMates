namespace LifeMates.Domain.Errors.User;

public class NotEnoughRightsError : ApplicationError
{
    public NotEnoughRightsError()
    {
        Message = "У Вас недостаточно прав";
    }
}