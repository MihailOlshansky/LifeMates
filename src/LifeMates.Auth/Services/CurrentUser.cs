using System.ComponentModel.Design;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Domain.Shared.Users;
using Microsoft.AspNetCore.Http;

namespace LifeMates.Auth.Services;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAccessTokenDecoder _accessTokenDecoder;

    public CurrentUser(IHttpContextAccessor httpContextAccessor, IAccessTokenDecoder accessTokenDecoder)
    {
        _httpContextAccessor = httpContextAccessor;
        _accessTokenDecoder = accessTokenDecoder;
    }

    public long GetUserId()
    {
        var token = GetToken();
        
        if (!long.TryParse(_accessTokenDecoder.GetUserId(token), out var userId))
        {
            throw new ArgumentException("Invalid access token");
        }

        return userId;
    }

    public string GetUserCredentialsId()
    {
        var token = GetToken();

        var id = _accessTokenDecoder.GetUserCredentialsId(token); 
        if (id == null)
        {
            throw new ArgumentException("Invalid access token");
        }

        return id;
    }

    public bool IsInRole(RoleTypes role)
    {
        var token = GetToken();

        var roleType = _accessTokenDecoder.GetUserRoleType(token);

        return roleType == role.ToString()
            ? true
            : false;
    }

    private string GetToken()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new InvalidOperationException("Http context doesn't exist");
        }

        var header = _httpContextAccessor.HttpContext.Request.Headers.Authorization.FirstOrDefault();

        var token = header?.Split(' ')[1];

        if (token is null)
        {
            throw new ArgumentException("Token was not found");
        }

        return token;
    }
}
