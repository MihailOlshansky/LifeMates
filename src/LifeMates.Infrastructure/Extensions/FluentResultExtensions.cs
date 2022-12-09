using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using LifeMates.Domain.Errors;

namespace LifeMates.Infrastructure.Extensions;

public static class FluentResultExtensions
{
    public static string? GetPropertyName(this IError error)
    {
        return error.Metadata.GetValueOrDefault(ApplicationError.PropertyNameKey) as string;
    }

    public static string? GetErrorCode(this IError error)
    {
        return error.Metadata.GetValueOrDefault(ApplicationError.ErrorCodeKey) as string;
    }

    public static IEnumerable<IError> FlattenErrors(this IEnumerable<IError> errors)
    {
        if (errors == null)
        {
            throw new ArgumentNullException(nameof(errors));
        }

        return errors.SelectMany(Flatten);
    }

    public static IEnumerable<IError> FlatErrors(this ResultBase result)
    {
        if (result == null)
        {
            throw new ArgumentNullException(nameof(result));
        }

        return result.Errors.FlattenErrors();
    }

    public static string AsString(this IEnumerable<Error> errors)
    {
        if (errors == null)
        {
            throw new ArgumentNullException(nameof(errors));
        }

        return string.Join(Environment.NewLine, errors.FlattenErrors().Select(x => x.Message));
    }

    public static void ThrowIfFailed(this ResultBase result)
    {
        if (result.IsSuccess)
        {
            return;
        }

        var failures = result
            .Errors
            .FlattenErrors()
            .Select(x => new ValidationFailure(x.GetPropertyName() ?? string.Empty, x.Message));

        throw new ValidationException(failures);
    }

    private static IEnumerable<IError> Flatten(this IError error)
    {
        yield return error;
        foreach (var reason in error.Reasons.SelectMany(Flatten))
        {
            yield return reason;
        }
    }
}