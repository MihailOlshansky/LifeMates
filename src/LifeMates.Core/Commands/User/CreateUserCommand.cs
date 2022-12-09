using System.Text.RegularExpressions;
using FluentResults;
using FluentValidation;
using LifeMates.Core.Commands.User.Models;
using LifeMates.Core.Services;
using LifeMates.Domain.Errors.Interest;
using LifeMates.Domain.Errors.User;
using LifeMates.Domain.Extensions;
using LifeMates.Domain.Shared.Users;
using LifeMates.Storage.SharedKernel.Repositories;
using MediatR;

namespace LifeMates.Core.Commands.User;

public record CreateUserCommand(
    string Email,
    string Password,
    string Name,
    string? Description,
    UserGender Gender,
    DateTime? Birthday,
    UserLocation? Location,
    UserSettings Settings,
    ICollection<long> Interests,
    ICollection<string> ImagesUrls,
    ICollection<UserContact> Contacts) 
    : IRequest<Result<CreateUserCommandResponse>>;

public record CreateUserCommandResponse(string AccessToken, string RefreshToken);

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private static Regex PasswordRegex =
        new("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,}$");
    
    public CreateUserCommandValidator(IValidator<UserLocation> locationValidator)
    {
        RuleFor(x => x.Email)
            .Must(email => EmailExtensions.IsValid(email))
            .WithMessage("Некорректный адрес электронной почты");
        
        RuleFor(x => x.Password)
            .Must(password => !string.IsNullOrWhiteSpace(password))
            .WithMessage("Пароль не может быть пустым")
            .MinimumLength(8)
            .WithMessage("Минимальная длина пароля - 8 символов")
            .Must(password => PasswordRegex.IsMatch(password))
            .WithMessage("Пароль должен содержать заглавные, строчные буквы и цифры");

        RuleFor(x => x.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Имя не может быть пустым");
        
        RuleFor(x => x.Description)
            .Must(desc => desc is null || !string.IsNullOrWhiteSpace(desc))
            .WithMessage("Описание не может быть пустым");

        When(x => x.Location is not null, () =>
        {
            RuleFor(x => x.Location)
                .SetValidator(locationValidator);
        });
    }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<CreateUserCommandResponse>>
{
    private readonly IUserCredentialsRepository _userCredentialsRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;
    private readonly IInterestRepository _interestRepository;

    public CreateUserCommandHandler(
        IUserCredentialsRepository userCredentialsRepository, 
        IUserRepository userRepository, 
        IAuthService authService,
        IInterestRepository interestRepository)
    {
        _userCredentialsRepository = userCredentialsRepository;
        _userRepository = userRepository;
        _authService = authService;
        _interestRepository = interestRepository;
    }

    public async Task<Result<CreateUserCommandResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userCredentialsRepository.Exists(request.Email, cancellationToken))
        {
            return Result.Fail(new UserWithEmailExistsError());
        }
        
        if (!await _interestRepository.Exists(request.Interests, cancellationToken))
        {
            return Result.Fail(new InterestNotFoundError());
        }
        
        var userLocation = request.Location is null
            ? null
            : new Domain.Models.Users.UserLocation(request.Location.Latitude, request.Location.Longitude);

        var userSettings = new Domain.Models.Users.UserSettings(request.Settings.ShowingGender);

        var userImages = request.ImagesUrls.Select(x => new Domain.Models.Users.UserImage(x)).ToList();

        var userContacts = request.Contacts.Select(x => new Domain.Models.Users.UserContact(x.Type, x.Value)).ToList();

        var userInterests = request.Interests.Select(x => new Domain.Models.Users.UserInterest(x)).ToList();

        var user = new Domain.Models.Users.User(
            request.Name,
            request.Description,
            request.Gender,
            UserStatus.Approved,
            request.Birthday,
            userLocation,
            userSettings,
            userImages,
            userContacts,
            userInterests);

        await _userRepository.Create(user, cancellationToken);
        
        var userCredentials = new Domain.Models.Users.UserCredentials(user.Id, request.Email);

        var result = await _userCredentialsRepository.Create(userCredentials, request.Password, cancellationToken);

        if (!result)
        {
            return Result.Fail(new UserCreationError());
        }

        var tokensResult = await _authService.GenerateTokens(request.Email, request.Password, cancellationToken);
        
        return Result.Ok(new CreateUserCommandResponse(
            tokensResult.Value.AccessToken, 
            tokensResult.Value.RefreshToken));
    }
}