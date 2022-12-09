using FluentResults;
using FluentValidation;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Domain.Errors.Interest;
using LifeMates.Domain.Errors.User;
using LifeMates.Domain.Shared.Users;
using LifeMates.Storage.SharedKernel.Repositories;
using MediatR;

namespace LifeMates.Core.Commands.Interest;

public record CreateInterestCommand(string Value) : IRequest<Result>;

public class CreateInterestCommandValidator : AbstractValidator<CreateInterestCommand>
{
    public CreateInterestCommandValidator()
    {
        RuleFor(x => x.Value)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("Интерес не может быть пустым");
    }
}

public class CreateInterestCommandHandler : IRequestHandler<CreateInterestCommand, Result>
{
    private readonly ICurrentUser _currentUser;
    private readonly IInterestRepository _interestRepository;

    public CreateInterestCommandHandler(
        ICurrentUser currentUser,
        IInterestRepository interestRepository)
    {
        _currentUser = currentUser;
        _interestRepository = interestRepository;
    }

    public async Task<Result> Handle(
        CreateInterestCommand request,
        CancellationToken cancellationToken)
    {
        if (!_currentUser.IsInRole(RoleTypes.Admin))
        {
            return Result.Fail(new NotEnoughRightsError());
        }

        if (await _interestRepository.Exists(request.Value, cancellationToken))
        {
            return Result.Fail(new InterestWithValueExistsError(request.Value));
        }

        var interest = new Domain.Models.Interests.Interest(request.Value);
        
        await _interestRepository.Create(interest, cancellationToken);
        
        return Result.Ok();
    }
}    