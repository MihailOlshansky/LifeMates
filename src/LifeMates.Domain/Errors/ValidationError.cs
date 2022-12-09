using FluentValidation;
using LifeMates.Domain.Constant;

namespace LifeMates.Domain.Errors;

public class ValidationError : ApplicationError
{
    public ValidationError(string? message = default)
    {
        Message = message ?? Constants.ErrorMessages.ModelValidation;
    }

    public ValidationError(ValidationException exception) : this(exception.Message)
    {
        var errors =
            exception.Errors.Select(x => new PropertyValidationError(x.PropertyName, x.ErrorMessage, x.ErrorCode));
        Reasons.AddRange(errors);
    }
}

public class PropertyValidationError : ApplicationError
{
    public PropertyValidationError(string propertyName, string message, string? validationErrorCode = null)
        : base(validationErrorCode)
    {
        Message = message;
        PropertyName = propertyName;
    }
}