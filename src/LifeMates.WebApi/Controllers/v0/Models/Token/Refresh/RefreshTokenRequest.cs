namespace LifeMates.WebApi.Controllers.v0.Models.Token.Refresh;

public record RefreshTokenRequest(string AccessToken, string RefreshToken);