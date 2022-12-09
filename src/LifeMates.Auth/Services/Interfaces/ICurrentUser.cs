using LifeMates.Domain.Shared.Users;

namespace LifeMates.Auth.Services.Interfaces;

public interface ICurrentUser
{
    long GetUserId();

    string GetUserCredentialsId();

    bool IsInRole(RoleTypes role);
}