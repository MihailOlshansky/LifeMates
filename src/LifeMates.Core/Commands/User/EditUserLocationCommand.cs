using FluentResults;
using FluentValidation;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Core.Commands.User.Models;
using LifeMates.Domain.Errors.User;
using LifeMates.Storage.SharedKernel.Repositories;
using MediatR;

namespace LifeMates.Core.Commands.User;

public class EditUserLocationCommand : IRequest<Result<Domain.Models.Users.User>>
{
    public UserLocation? Location { get; set; }
}

public class EditUserLocationCommandValidator : AbstractValidator<EditUserLocationCommand>
{
    public EditUserLocationCommandValidator(IValidator<UserLocation> validator)
    {
        When(x => x.Location is not null, () =>
        {
            RuleFor(x => x.Location)
                .SetValidator(validator);
        });
    }
}

public class EditUserLocationCommandHandler : IRequestHandler<EditUserLocationCommand, Result<Domain.Models.Users.User>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IUserRepository _userRepository;
    
    public EditUserLocationCommandHandler(
        ICurrentUser currentUser, 
        IUserRepository userRepository)
    {
        _currentUser = currentUser;
        _userRepository = userRepository;
    }

    public async Task<Result<Domain.Models.Users.User>> Handle(
        EditUserLocationCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId();

        var user = await _userRepository.Get(userId, cancellationToken);

        if (user is null)
        {
            Result.Fail(new UserNotFoundError());
        }
        
        var userLocation =
            request.Location is null
            ? null
            : new Domain.Models.Users.UserLocation(request.Location.Latitude, request.Location.Longitude);
        
        var updateResult = user!.Update(userLocation);

        if (updateResult.IsFailed)
        {
            return Result.Fail(new UserUpdateError());
        }

        if (updateResult.Value)
        {
            await _userRepository.Commit(cancellationToken);
        }
        
        return Result.Ok(user);
    }
}

