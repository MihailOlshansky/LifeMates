namespace LifeMates.Domain.Errors.Interest;

public class InterestWithValueExistsError : ApplicationError
{
    public InterestWithValueExistsError(string value)
    {
        Message = "Уже есть интерес со значением " + value;
    }
}