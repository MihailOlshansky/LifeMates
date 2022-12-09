using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace LifeMates.WebApi.Controllers.v0.Models;

/// <summary>
/// Общая модель ответа, содержащая ошибку
/// </summary>
public class ApiBadResponse : IActionResult
{
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    [Required]
    public string Message { get; }
    /// <summary>
    /// Код ошибки
    /// </summary>
    public string? ErrorCode { get; }
    /// <summary>
    /// Дополнительная информация об ошибке
    /// </summary>
    public IDictionary<string, string[]>? Arguments { get; }
    public IDictionary<string, object>? Extensions { get; }

    [Required]
    public int HttpStatusCode { get; }

    public ApiBadResponse(string message,
        int httpStatusCode,
        string? errorCode = null ,
        IDictionary<string, string[]>? arguments = null,
        IDictionary<string, object>? extensions = null)
    {
        Message = message;
        ErrorCode = errorCode;
        Arguments = arguments;
        Extensions = extensions;

        HttpStatusCode = httpStatusCode;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var objectResult = new ObjectResult(this)
        {
            StatusCode = HttpStatusCode
        };

        await objectResult.ExecuteResultAsync(context);
    }
}