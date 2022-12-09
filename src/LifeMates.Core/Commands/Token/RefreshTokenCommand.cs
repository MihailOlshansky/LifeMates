using FluentResults;
using LifeMates.Auth.Services;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Domain.Errors.Token;
using LifeMates.Domain.Shared.Users;
using LifeMates.Storage.SharedKernel.Repositories;
using MediatR;

namespace LifeMates.Core.Commands.Token;

public record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<Result<RefreshTokenCommandResponse>>;

public record RefreshTokenCommandResponse(string AccessToken, string RefreshToken);

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenCommandResponse>>
{
    private readonly ITokenService _tokenService;
    private readonly IAccessTokenDecoder _accessTokenDecoder;
    private readonly IRefreshTokenValidator _refreshTokenValidator;
    private readonly IUserCredentialsRepository _userCredentialsRepository;


    public RefreshTokenCommandHandler(ITokenService tokenService, IAccessTokenDecoder accessTokenDecoder, IRefreshTokenValidator refreshTokenValidator, IUserCredentialsRepository userCredentialsRepository)
    {
        _tokenService = tokenService;
        _accessTokenDecoder = accessTokenDecoder;
        _refreshTokenValidator = refreshTokenValidator;
        _userCredentialsRepository = userCredentialsRepository;
    }

    public async Task<Result<RefreshTokenCommandResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (!_refreshTokenValidator.Validate(request.RefreshToken))
        {
            return Result.Fail(new InvalidRefreshToken());
        }

        var userId = _accessTokenDecoder.GetUserCredentialsId(request.AccessToken);

        if (userId is null)
        {
            return Result.Fail(new InvalidAccessToken());
        }
        
        var userCredentials = await _userCredentialsRepository.FindById(userId, cancellationToken);

        if (userCredentials is null)
        {
            return Result.Fail(new InvalidAccessToken());
        }
        
        var currentRefreshToken = await _userCredentialsRepository.GetRefreshToken(userCredentials, cancellationToken);

        if (currentRefreshToken != request.RefreshToken)
        {
            return Result.Fail(new InvalidRefreshToken());
        }

        var accessToken = 
            !await _userCredentialsRepository.IsInRole(userCredentials, RoleTypes.Admin.ToString(), cancellationToken)
                ? _tokenService.GenerateAccessToken(userCredentials)
                : _tokenService.GenerateAccessToken(userCredentials, RoleTypes.Admin);
        var refreshToken = _tokenService.GenerateRefreshToken();
        await _userCredentialsRepository.SetRefreshToken(userCredentials, refreshToken, cancellationToken);

        return Result.Ok(new RefreshTokenCommandResponse(accessToken, refreshToken));
    }
}