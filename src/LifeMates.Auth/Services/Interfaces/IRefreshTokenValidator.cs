namespace LifeMates.Auth.Services.Interfaces;

public interface IRefreshTokenValidator
{
    bool Validate(string refreshToken);
}