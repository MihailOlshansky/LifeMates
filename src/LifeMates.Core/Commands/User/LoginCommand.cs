using FluentResults;
using LifeMates.Core.Services;
using MediatR;

namespace LifeMates.Core.Commands.User;

public record LoginCommand(string Email, string Password) : IRequest<Result<LoginCommandResponse>>;

public record LoginCommandResponse(string AccessToken, string RefreshToken);

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    private readonly IAuthService _authService;
    
    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.GenerateTokens(request.Email, request.Password, cancellationToken);

        return result.IsSuccess
            ? Result.Ok(new LoginCommandResponse(result.Value.AccessToken, result.Value.RefreshToken))
            : Result.Fail(result.Errors);
    }
}