using FluentResults;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Domain.Errors.User;
using LifeMates.Domain.Models.Chats;
using LifeMates.Domain.Models.Users;
using LifeMates.Storage.SharedKernel;
using LifeMates.Storage.SharedKernel.Repositories;
using MediatR;

namespace LifeMates.Core.Commands.Match;

public record LikeCommand(long UserId) : IRequest<Result<LikeCommandResponse>>;
public record LikeCommandResponse(bool IsItMutual);

public class LikeCommandHandler : IRequestHandler<LikeCommand, Result<LikeCommandResponse>>
{
    private readonly IUserLikesRepository _likesRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IChatRepository _chatRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public LikeCommandHandler(
        IUserLikesRepository likesRepository, 
        IUserRepository userRepository, 
        ICurrentUser currentUser,
        IChatRepository chatRepository,
        IUnitOfWork unitOfWork)
    {
        _likesRepository = likesRepository;
        _userRepository = userRepository;
        _currentUser = currentUser;
        _chatRepository = chatRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LikeCommandResponse>> Handle(LikeCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId();

        if (!await _userRepository.Exists(request.UserId, cancellationToken))
        {
            return Result.Fail(new UserNotFoundError(request.UserId));
        }

        var exist = await _likesRepository.Exist(userId, request.UserId, cancellationToken);

        if (!exist)
        {
            var like = new UserLikes(userId, request.UserId);
            _likesRepository.Create(like);
        }

        var isItMutual = await _likesRepository.Exist(request.UserId, userId, cancellationToken);

        if (isItMutual)
        {
            var chat = new Chat(
                new ChatUser(userId, true),
                new ChatUser(request.UserId, false));
            _chatRepository.Create(chat);
        }

        await _unitOfWork.Commit(cancellationToken);
        
        return Result.Ok(new LikeCommandResponse(isItMutual));
    }
}