using FluentResults;
using LifeMates.Auth.Services;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Domain.Errors.Token;
using LifeMates.Storage.SharedKernel.Repositories;
using MediatR;

namespace LifeMates.Core.Commands.Token;

public record RevokeTokenCommand(string AccessToken, string RefreshToken) : IRequest<Result>;

public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, Result>
{
    private readonly ITokenService _tokenService;
    private readonly IAccessTokenDecoder _accessTokenDecoder;
    private readonly IRefreshTokenValidator _refreshTokenValidator;
    private readonly IUserCredentialsRepository _userCredentialsRepository;

    public RevokeTokenCommandHandler(ITokenService tokenService, IAccessTokenDecoder accessTokenDecoder, IRefreshTokenValidator refreshTokenValidator, IUserCredentialsRepository userCredentialsRepository)
    {
        _tokenService = tokenService;
        _accessTokenDecoder = accessTokenDecoder;
        _refreshTokenValidator = refreshTokenValidator;
        _userCredentialsRepository = userCredentialsRepository;
    }

    public async Task<Result> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
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
        
        var result = await _userCredentialsRepository.RemoveRefreshToken(userCredentials, cancellationToken);

        return result 
            ? Result.Ok() 
            : Result.Fail(new RevokeRefreshTokenError());
    }
}