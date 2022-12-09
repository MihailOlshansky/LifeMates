namespace LifeMates.Domain.Constant;

public static class Constants
{
    public static class ErrorMessages
    {
        public const string ModelValidation = "Ошибка валидации";
    }

    public static class Users
    {
        public static class Match
        {
            public static readonly TimeSpan DislikeCooldown = new(30, 0, 0, 0, 0);
            public const double Radius = 0.1;
        }
    }
}