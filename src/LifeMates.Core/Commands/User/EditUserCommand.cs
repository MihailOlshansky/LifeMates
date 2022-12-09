using FluentResults;
using FluentValidation;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Core.Commands.User.Models;
using LifeMates.Domain.Errors.Interest;
using LifeMates.Domain.Errors.User;
using LifeMates.Domain.Shared.Users;
using LifeMates.Storage.SharedKernel.Repositories;
using MediatR;

namespace LifeMates.Core.Commands.User;

public record EditUserCommand(
    string Name,
    string? Description,
    UserGender Gender,
    DateTime? Birthday,
    UserSettings Settings,
    ICollection<long> Interests,
    ICollection<string> ImagesUrls,
    ICollection<UserContact> Contacts) 
    : IRequest<Result<Domain.Models.Users.User>>;

public class EditUserCommandValidator : AbstractValidator<EditUserCommand>
{
    public EditUserCommandValidator(IValidator<UserLocation> locationValidator)
    {
        RuleFor(x => x.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Имя не может быть пустым");
        
        RuleFor(x => x.Description)
            .Must(desc => desc is null || !string.IsNullOrWhiteSpace(desc))
            .WithMessage("Описание не может быть пустым");
    }
}

public class EditUserCommandHandler : IRequestHandler<EditUserCommand, Result<Domain.Models.Users.User>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IUserRepository _userRepository;
    private readonly IInterestRepository _interestRepository;
    
    public EditUserCommandHandler(
        ICurrentUser currentUser, 
        IUserRepository userRepository, 
        IInterestRepository interestRepository)
    {
        _currentUser = currentUser;
        _userRepository = userRepository;
        _interestRepository = interestRepository;
    }

    public async Task<Result<Domain.Models.Users.User>> Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId();

        var user = await _userRepository.Get(userId, cancellationToken);

        if (user is null)
        {
            Result.Fail(new UserNotFoundError());
        }
        
        if (!await _interestRepository.Exists(request.Interests, cancellationToken))
        {
            return Result.Fail(new InterestNotFoundError());
        }

        var userSettings = new Domain.Models.Users.UserSettings(request.Settings.ShowingGender);

        var userImages = request.ImagesUrls.Select(x => new Domain.Models.Users.UserImage(x)).ToList();

        var userContacts = request.Contacts.Select(x => new Domain.Models.Users.UserContact(x.Type, x.Value)).ToList();

        var userInterests = request.Interests.Select(x => new Domain.Models.Users.UserInterest(x)).ToList();

        var updateResult = user!.Update(
            request.Name,
            request.Description,
            request.Gender,
            request.Birthday,
            userSettings,
            userImages,
            userContacts,
            userInterests);

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