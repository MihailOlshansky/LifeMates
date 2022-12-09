using FluentResults;
using FluentValidation;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Domain.ReadOnly.Chats;
using LifeMates.Storage.SharedKernel;
using LifeMates.Storage.SharedKernel.ReadOnlyRepositories;
using LifeMates.Storage.SharedKernel.Repositories;
using MediatR;

namespace LifeMates.Core.Queries.Chat;

public record GetMatchesQuery(int Offset, int Limit) : IRequest<Result<GetMatchesQueryResponse>>;
public record GetMatchesQueryResponse(ICollection<(MatchView Match, bool IsSeen)> Matches, int Count);

public class GetMatchesQueryValidator : AbstractValidator<GetMatchesQuery>
{
    public GetMatchesQueryValidator()
    {
        RuleFor(x => x.Limit)
            .GreaterThan(-1)
            .LessThan(100);
    }
}

public class GetMatchesQueryHandler : IRequestHandler<GetMatchesQuery, Result<GetMatchesQueryResponse>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IChatReadOnlyRepository _chatReadOnlyRepository;
    private readonly IUserChatRepository _userChatRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetMatchesQueryHandler(
        ICurrentUser currentUser,
        IChatReadOnlyRepository chatReadOnlyRepository, 
        IUserChatRepository userChatRepository,
        IUnitOfWork unitOfWork)
    {
        _currentUser = currentUser;
        _chatReadOnlyRepository = chatReadOnlyRepository;
        _userChatRepository = userChatRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetMatchesQueryResponse>> Handle(
        GetMatchesQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId();

        var count = await _chatReadOnlyRepository.CountMatches(userId, cancellationToken);

        if (request.Limit == 0)
        {
            return Result.Ok(new GetMatchesQueryResponse(new List<(MatchView Match, bool IsSeen)>(), count));
        }
        
        var result = await _chatReadOnlyRepository.GetMatches(userId, request.Offset, request.Limit, cancellationToken);

        var userChats = (await _userChatRepository.Get(userId, result.Select(x => x.Id).ToList(), cancellationToken))
            .ToDictionary(x => x.ChatId, x => x);

        var response = result.Select(x => (x, userChats[x.Id].IsSeen)).ToList();
        
        if (userChats.Values.Select(x => x.Update(true)).Any(x => x.IsSuccess && x.Value))
        {
            await _unitOfWork.Commit(cancellationToken);
        }

        return Result.Ok(new GetMatchesQueryResponse(response, count));
    }
}
