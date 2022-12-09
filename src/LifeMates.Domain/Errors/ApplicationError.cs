using FluentResults;

namespace LifeMates.Domain.Errors;

public abstract class ApplicationError : Error
{
    public const string AggregateError = nameof(AggregateError);
    public const string ErrorCodeKey = "ErrorCode";
    public const string PropertyNameKey = "PropertyName";

    protected ApplicationError()
    {
        Metadata[ErrorCodeKey] = GetType().Name;
    }

    protected ApplicationError(string? errorCode)
    {
        Metadata[ErrorCodeKey] = !string.IsNullOrEmpty(errorCode) ? errorCode : GetType().Name;
    }

    public string ErrorCode => Metadata[ErrorCodeKey] as string ?? throw new InvalidOperationException();

    public string? PropertyName
    {
        get => this.GetPropertyName();
        protected init => Metadata[PropertyNameKey] = value;
    }
}

internal static class ErrorExtensions
{
    public static string? GetPropertyName(this Error error)
    {
        return error.Metadata.GetValueOrDefault(ApplicationError.PropertyNameKey) as string;
    }
}