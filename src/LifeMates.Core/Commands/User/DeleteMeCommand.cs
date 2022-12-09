using FluentResults;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Domain.Errors.User;
using LifeMates.Domain.Shared.Users;
using LifeMates.Storage.SharedKernel.Repositories;
using MediatR;

namespace LifeMates.Core.Commands.User;

public record DeleteMeCommand() : IRequest<Result>;

public class DeleteMeCommandHandler : IRequestHandler<DeleteMeCommand, Result>
{
    private readonly ICurrentUser _currentUser;
    private readonly IUserRepository _userRepository;

    public DeleteMeCommandHandler(ICurrentUser currentUser, IUserRepository userRepository)
    {
        _currentUser = currentUser;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(DeleteMeCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId();
        var user = await _userRepository.Get(userId, cancellationToken);
        if (user == null)
        {
            return Result.Fail(new CredentialsNotFoundError());
        }
        
        var result = user.Update(UserStatus.Deleted);

        if (result.Value)
        {
            await _userRepository.Commit(cancellationToken);
        }
        
        return Result.Ok();
    }
}