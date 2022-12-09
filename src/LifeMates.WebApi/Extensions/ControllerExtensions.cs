using FluentResults;
using LifeMates.Domain.Errors;
using LifeMates.Infrastructure.Constant;
using LifeMates.Infrastructure.Extensions;
using LifeMates.WebApi.Controllers.v0.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LifeMates.WebApi.Extensions;

public static class ControllerExtensions
{
    public static IActionResult Process(this ControllerBase controller, Result result)
    {
        return ProcessResponse(controller, result);
    }
    
    public static IActionResult Process<T>(this ControllerBase controller, Result<T> result)
    {
        return ProcessResponse(controller, result);
    }

    private static IActionResult ProcessResponse(ControllerBase controller, ResultBase result)
    {
        if (result.IsSuccess)
        {
            return controller.Ok();
        }

        return ProcessError(result);
    }
    
    private static IActionResult ProcessError(ResultBase result)
    {
        var errors = result
            .Errors
            .FlattenErrors()
            .ToArray();

        var arguments = errors
            .GroupBy
            (
                x => x.GetPropertyName() ?? string.Empty,
                x => x.Message
            )
            .ToDictionary(x => x.Key, x => x.ToArray());

        var extensions = errors
            .SelectMany(x => x.Metadata)
            .Where(x => !x.Key.Equals(ApplicationError.PropertyNameKey) &&
                        !x.Key.Equals(ApplicationError.ErrorCodeKey))
            .GroupBy(x => x.Key, x => x.Value)
            .ToDictionary(x => x.Key, x => x.First());

        var (errorCode, message) = GetErrorCodeAndMessage(errors);

        return new ApiBadResponse
        (
            message,
            StatusCodes.Status400BadRequest,
            errorCode,
            arguments,
            extensions
        );
    }

    private static (string ErrorCode, string Message) GetErrorCodeAndMessage(IReadOnlyList<IError> errors)
    {
        var validationError = errors.OfType<ValidationError>().FirstOrDefault();

        if (validationError != null)
        {
            // если есть ошибка валидации, то отдаём её в первую очередь для обеспечения совместимости
            var errorCode = validationError.ErrorCode;
            var message = validationError.Message;

            return (errorCode, message);
        }

        if (errors.Count == 1)
        {
            // если ошибка только одна, то отдаём её код и сообщение
            var errorCode = errors[0].GetErrorCode() ?? Constants.ErrorMessages.ValidationErrorCode;
            var message = errors[0].Message;

            return (errorCode, message);
        }

        // если ошибок несколько, то отдаём в коде AggregateError
        return (ApplicationError.AggregateError, Constants.ErrorMessages.ModelValidation);
    }
}