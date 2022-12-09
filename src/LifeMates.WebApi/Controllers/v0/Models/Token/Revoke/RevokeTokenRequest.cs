namespace LifeMates.WebApi.Controllers.v0.Models.Token.Invoke;

public record RevokeTokenRequest(string AccessToken, string RefreshToken);