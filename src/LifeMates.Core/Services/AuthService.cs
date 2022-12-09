using FluentResults;
using LifeMates.Auth.Services;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Domain.Errors.User;
using LifeMates.Domain.Shared.Users;
using LifeMates.Storage.SharedKernel.Repositories;

namespace LifeMates.Core.Services;

public class AuthService : IAuthService
{
    private readonly IUserCredentialsRepository _userCredentialsRepository;
    private readonly ITokenService _tokenService;
    
    public AuthService(IUserCredentialsRepository userCredentialsRepository, ITokenService tokenService)
    {
        _userCredentialsRepository = userCredentialsRepository;
        _tokenService = tokenService;
    }
    
    public async Task<Result<(string AccessToken, string RefreshToken)>> GenerateTokens(
        string email, string password, CancellationToken cancellationToken)
    {
        var userCredentials = await _userCredentialsRepository.FindByEmail(email, cancellationToken);
        if (userCredentials == null)
        {
            return Result.Fail(new UserNotFoundError());
        }

        var result = await _userCredentialsRepository.CheckPasswordSignIn(userCredentials, password, cancellationToken);

        if (!result)
        {
            return Result.Fail(new InvalidUserCredentialsError());
        }
        
        var accessToken = 
            !await _userCredentialsRepository.IsInRole(userCredentials, RoleTypes.Admin.ToString(), cancellationToken)
                ? _tokenService.GenerateAccessToken(userCredentials)
                : _tokenService.GenerateAccessToken(userCredentials, RoleTypes.Admin);
        
        var refreshToken = _tokenService.GenerateRefreshToken();
            
        await _userCredentialsRepository.SetRefreshToken(
            userCredentials,
            refreshToken,
            cancellationToken);
            
        return Result.Ok((accessToken, refreshToken));
    }
}