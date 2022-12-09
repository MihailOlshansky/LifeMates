using FluentResults;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Domain.Errors.Chats;
using LifeMates.Storage.SharedKernel;
using LifeMates.Storage.SharedKernel.Repositories;
using MediatR;

namespace LifeMates.Core.Commands.Chats;

public record CreateMessageCommand(long ChatId, string Message) : IRequest<Result>;

public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, Result>
{
    private readonly IChatRepository _chatRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public CreateMessageCommandHandler(IChatRepository chatRepository, IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _chatRepository = chatRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUser.GetUserId();
        var chat = await _chatRepository.Get(request.ChatId, cancellationToken);
        if (chat is null || !chat.IsUserChat(currentUserId))
        {
            return Result.Fail(new ChatNotFoundError(request.ChatId));
        }

        chat.AddMessage(currentUserId, request.Message);
        await _unitOfWork.Commit(cancellationToken);
        
        return Result.Ok();
    }
}