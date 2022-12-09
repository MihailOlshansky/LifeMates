namespace LifeMates.Infrastructure.Constant;

public static class Constants
{
    public static class ErrorMessages
    {
        public const string ValidationErrorCode = "ValidationError";
        public const string ModelValidation = "Ошибки валидации модели";
    }
    
    public static class Tokens
    {
        public static class Providers
        {
            public const string LifeMates = "LifeMates";
        }

        public static class Types
        {
            public const string RefreshToken = "RefreshToken";
        }
    }
}