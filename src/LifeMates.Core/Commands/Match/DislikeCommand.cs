using FluentResults;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Domain.Errors.Match;
using LifeMates.Domain.Errors.User;
using LifeMates.Domain.Models.Users;
using LifeMates.Storage.SharedKernel;
using LifeMates.Storage.SharedKernel.Repositories;
using MediatR;

namespace LifeMates.Core.Commands.Match;

public record DislikeCommand(long UserId) : IRequest<Result>;

public class DislikeCommandHandler : IRequestHandler<DislikeCommand, Result>
{
    private readonly IUserDislikeRepository _dislikeRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    
    public DislikeCommandHandler(
        IUserDislikeRepository dislikeRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _dislikeRepository = dislikeRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(DislikeCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId();

        if (!await _userRepository.Exists(request.UserId, cancellationToken))
        {
            return Result.Fail(new UserNotFoundError(request.UserId));
        }

        var dislike = await _dislikeRepository.Get(userId, request.UserId, cancellationToken);

        if (dislike is not null)
        {
            var result = dislike.Update();

            if (result.IsFailed)
            {
                return Result.Fail(new UpdateDislikeError());
            }
        }
        else
        {
            dislike = new UserDislikes(userId, request.UserId);
            
            _dislikeRepository.Create(dislike);
        }

        await _unitOfWork.Commit(cancellationToken);
        
        return Result.Ok();
    }
}