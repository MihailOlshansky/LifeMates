using Microsoft.AspNetCore.Mvc;

namespace LifeMates.Auth.Extensions;

public static class ControllerExtensions
{
    public static Guid GetUserId(this ControllerBase controller)
    {
        return Guid.Parse(controller.User.Claims.First(i => i.Type == "UserId").Value);
    }
}