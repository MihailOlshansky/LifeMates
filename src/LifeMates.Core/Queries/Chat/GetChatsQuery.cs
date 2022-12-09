using FluentResults;
using FluentValidation;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Domain.ReadOnly.Chats;
using LifeMates.Storage.SharedKernel.ReadOnlyRepositories;
using MediatR;

namespace LifeMates.Core.Queries.Chat;

public record GetChatsQuery(int Offset, int Limit) : IRequest<Result<GetChatsQueryResponse>>;
public record GetChatsQueryResponse(ICollection<ChatView> Chats, int Count);

public class GetChatsQueryValidator : AbstractValidator<GetMatchesQuery>
{
    public GetChatsQueryValidator()
    {
        RuleFor(x => x.Limit)
            .GreaterThan(0)
            .LessThan(100);
    }
}

public class GetChatsQueryHandler : IRequestHandler<GetChatsQuery, Result<GetChatsQueryResponse>>
{
    private readonly IChatReadOnlyRepository _chatReadOnlyRepository;
    private readonly ICurrentUser _currentUser;

    public GetChatsQueryHandler(IChatReadOnlyRepository chatReadOnlyRepository, ICurrentUser currentUser)
    {
        _chatReadOnlyRepository = chatReadOnlyRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<GetChatsQueryResponse>> Handle(GetChatsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId();

        var count = await _chatReadOnlyRepository.CountChats(userId, cancellationToken);
        var chats = await _chatReadOnlyRepository.GetChats(userId, request.Offset, request.Limit, cancellationToken);

        // костыль, но лучше не хочу делать
        foreach (var chat in chats)
        {
            chat.Message.IsSeen = chat.Message.IsSeen || chat.Message.User.Id == userId;
        }

        return Result.Ok(new GetChatsQueryResponse(chats, count));
    }
}