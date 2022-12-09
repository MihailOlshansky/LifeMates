namespace LifeMates.WebApi.Controllers.v0.Models.User.Login;

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}