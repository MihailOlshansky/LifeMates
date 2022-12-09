using FluentResults;
using FluentValidation;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Core.Queries.Common;
using LifeMates.Domain.Constant;
using LifeMates.Domain.Errors.User;
using LifeMates.Domain.Models.Users;
using LifeMates.Domain.ReadOnly.Users;
using LifeMates.Storage.SharedKernel.ReadOnlyRepositories;
using LifeMates.Storage.SharedKernel.Repositories;
using MediatR;

namespace LifeMates.Core.Queries.Users;

public record SearchUserQuery(int Limit, int Offset) : IRequest<Result<SearchUserQueryResponse>>;

public record SearchUserQueryResponse(ICollection<UserView> UserViews, UserLocation? CurrentUserLocation);

public class SearchUserQueryValidator : AbstractValidator<SearchUserQuery>
{
    public SearchUserQueryValidator(IValidator<IPagination> paginationValidation)
    {
        RuleFor(x => x.Limit)
            .GreaterThan(-1)
            .LessThan(100);
        
        RuleFor(x => x.Offset)
            .GreaterThan(-1)
            .LessThan(100);
    }
}

public class SearchUserQueryHandler : IRequestHandler<SearchUserQuery, Result<SearchUserQueryResponse>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserSettingsRepository _userSettingsRepository;
    private readonly IUserLocationRepository _userLocationRepository;
    
    public SearchUserQueryHandler(
        ICurrentUser currentUser, 
        IUserReadOnlyRepository userReadOnlyRepository, 
        IUserSettingsRepository userSettingsRepository, 
        IUserLocationRepository userLocationRepository)
    {
        _currentUser = currentUser;
        _userReadOnlyRepository = userReadOnlyRepository;
        _userSettingsRepository = userSettingsRepository;
        _userLocationRepository = userLocationRepository;
    }

    public async Task<Result<SearchUserQueryResponse>> Handle(SearchUserQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId();

        var userSettings = await _userSettingsRepository.GetByUserId(userId, cancellationToken);

        if (userSettings is null)
        {
            return Result.Fail(new UserSettingsNotFoundError());
        }
        
        var currentUserLocation = await _userLocationRepository.GetByUserId(userId, cancellationToken);

        var users = await _userReadOnlyRepository.Search(
            new SearchFilter()
            {
                SearchForUserId = userId,
                Limit = request.Limit,
                Offset = request.Offset,
                // todo by user settings
                Radius = Constants.Users.Match.Radius,
                Latitude = currentUserLocation?.Latitude,
                Longitude = currentUserLocation?.Longitude,
                SearchingGender = userSettings.ShowingGender
            }, 
            cancellationToken);

        return Result.Ok(new SearchUserQueryResponse(users, currentUserLocation));
    }
}