using FluentResults;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Domain.Errors.Interest;
using LifeMates.Domain.Errors.User;
using LifeMates.Domain.Shared.Users;
using LifeMates.Storage.SharedKernel.Repositories;
using MediatR;

namespace LifeMates.Core.Commands.Interest;

public record DeleteInterestCommand(long Id) : IRequest<Result>;

public class DeleteInterestCommandHandler : IRequestHandler<DeleteInterestCommand, Result>
{
    private readonly ICurrentUser _currentUser;
    private readonly IInterestRepository _interestRepository;

    public DeleteInterestCommandHandler(
        ICurrentUser currentUser,
        IInterestRepository interestRepository)
    {
        _currentUser = currentUser;
        _interestRepository = interestRepository;
    }

    public async Task<Result> Handle(DeleteInterestCommand request,
        CancellationToken cancellationToken)
    {
        if (!_currentUser.IsInRole(RoleTypes.Admin))
        {
            return Result.Fail(new NotEnoughRightsError());
        }

        var interest = _interestRepository.GetById(request.Id, cancellationToken);
        if (interest.Result is null)
        {
            return Result.Fail(new InterestNotFoundError());
        }

        await _interestRepository.Remove(interest.Result, cancellationToken);
        
        return Result.Ok();
    }
}    