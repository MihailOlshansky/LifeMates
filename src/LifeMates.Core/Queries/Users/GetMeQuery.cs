using FluentResults;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Domain.Errors.User;
using LifeMates.Domain.ReadOnly.Users;
using LifeMates.Storage.SharedKernel.ReadOnlyRepositories;
using MediatR;

namespace LifeMates.Core.Queries.Users;

public record GetMeQuery : IRequest<Result<MeView>>;

public class GetMeQueryHandler : IRequestHandler<GetMeQuery, Result<MeView>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;

    public GetMeQueryHandler(ICurrentUser currentUser, IUserReadOnlyRepository userReadOnlyRepository)
    {
        _currentUser = currentUser;
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public async Task<Result<MeView>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId();

        var user = await _userReadOnlyRepository.GetMe(userId, cancellationToken);

        return user is null 
            ? Result.Fail(new UserNotFoundError()) 
            : Result.Ok(user);
    }
}