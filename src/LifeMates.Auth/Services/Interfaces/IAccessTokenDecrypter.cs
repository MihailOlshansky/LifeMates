namespace LifeMates.Auth.Services.Interfaces;

public interface IAccessTokenDecoder
{
    string? GetUserCredentialsId(string accessToken);
    string? GetUserId(string accessToken);

    string? GetUserRoleType(string accessToken);
}