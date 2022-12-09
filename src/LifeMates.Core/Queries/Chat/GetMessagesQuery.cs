using FluentResults;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Core.Queries.Common;
using LifeMates.Domain.Errors.Chats;
using LifeMates.Domain.ReadOnly.Chats;
using LifeMates.Storage.SharedKernel;
using LifeMates.Storage.SharedKernel.Repositories;
using MediatR;

namespace LifeMates.Core.Queries.Chat;

public class GetMessagesQuery : IPagination,
    IRequest<Result<GetMessagesQueryResponse>>
{
    public long ChatId { get; set; }
    public int Limit { get; set; }
    public int Offset { get; set; }
}

public record GetMessagesQueryResponse(ICollection<MessageView> Messages, int Count);

public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, Result<GetMessagesQueryResponse>>
{
    private readonly IChatRepository _chatRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public GetMessagesQueryHandler(IChatRepository chatRepository, IMessageRepository messageRepository, IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _chatRepository = chatRepository;
        _messageRepository = messageRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<GetMessagesQueryResponse>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUser.GetUserId();
        if (!await _chatRepository.IsUserChat(request.ChatId, currentUserId, cancellationToken))
        {
            return Result.Fail(new ChatNotFoundError(request.ChatId));
        }
        
        var messages = await _messageRepository.Get(request.ChatId, request.Limit, request.Offset, cancellationToken);
        var count = await _messageRepository.Count(request.ChatId, cancellationToken);

        var response = new GetMessagesQueryResponse(
            messages.Select(x => new MessageView
            {
                Id = x.Id,
                Content = x.Content,
                CreatedAt = x.CreatedAt,
                IsSeen = x.IsSeen || x.UserId == currentUserId,
                UserId = x.UserId
            }).ToList(), count);
        
        var hasUpdated = false;
        foreach (var message in messages)
        {
            if (message.UserId != currentUserId)
            {
                var result = message.Update(true);
                hasUpdated = hasUpdated || result.IsSuccess;
            }
        }

        if (hasUpdated)
        {
            await _unitOfWork.Commit(cancellationToken);
        }

        return Result.Ok(response);
    }
}