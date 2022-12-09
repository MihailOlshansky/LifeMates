using FluentResults;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Domain.Errors.User;
using LifeMates.Domain.Models.Users;
using LifeMates.Domain.ReadOnly.Users;
using LifeMates.Storage.SharedKernel.ReadOnlyRepositories;
using LifeMates.Storage.SharedKernel.Repositories;
using MediatR;

namespace LifeMates.Core.Queries.Users;

public record GetUserQuery(long Id) : IRequest<Result<GetUserQueryResponse>>;

public record GetUserQueryResponse(UserView UserView, UserLocation? CurrentUserLocation);

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<GetUserQueryResponse>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserLocationRepository _userLocationRepository;

    public GetUserQueryHandler(
        ICurrentUser currentUser, 
        IUserReadOnlyRepository userReadOnlyRepository, 
        IUserLocationRepository userLocationRepository)
    {
        _currentUser = currentUser;
        _userReadOnlyRepository = userReadOnlyRepository;
        _userLocationRepository = userLocationRepository;
    }

    public async Task<Result<GetUserQueryResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId();

        var currentUserLocation = await _userLocationRepository.GetByUserId(userId, cancellationToken);

        var user = await _userReadOnlyRepository.Get(request.Id, cancellationToken);

        return user is null 
            ? Result.Fail(new UserNotFoundError()) 
            : Result.Ok(new GetUserQueryResponse(user, currentUserLocation));
    }
}