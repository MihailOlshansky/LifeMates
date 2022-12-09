using FluentResults;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Domain.Errors.User;
using LifeMates.Domain.Shared.Users;
using LifeMates.Storage.SharedKernel.Repositories;
using MediatR;

namespace LifeMates.Core.Commands.User;

public record EditUserStatusCommand(long UserId, UserStatus NewStatus) : IRequest<Result>;

public class EditUserStatusCommandHandler : IRequestHandler<EditUserStatusCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUser _currentUser;

    public EditUserStatusCommandHandler(
        IUserRepository userRepository, ICurrentUser currentUser)
    {
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(EditUserStatusCommand request, CancellationToken cancellationToken)
    {
        if (request.NewStatus == UserStatus.Deleted)
        {
            return Result.Fail(new WrongNewStatusError());
        }

        if (!_currentUser.IsInRole(RoleTypes.Admin))
        {
            return Result.Fail(new NotEnoughRightsError());
        }

        var updateUser = await _userRepository.Get(request.UserId, cancellationToken);
        if (updateUser is null)
        {
            return Result.Fail(new UserNotFoundError());
        }

        var result = updateUser.Update(request.NewStatus);
        if (result.Value)
        {
            await _userRepository.Commit(cancellationToken);
        }
        
        return Result.Ok();
    }
    
}