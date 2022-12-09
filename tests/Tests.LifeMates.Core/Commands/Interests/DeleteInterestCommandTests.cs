using FluentAssertions;
using LifeMates.Core.Commands.Interest;
using LifeMates.Core.Mediatr.Behaviors;
using LifeMates.Domain.Models.Interests;
using LifeMates.Storage;
using LifeMates.Storage.Repositories;
using LifeMates.Storage.SharedKernel.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Tests.LifeMates.Core.Commands.Interests;

public class DeleteInterestCommandTests
{
    private LifematesDbContext _dbContext;
    private IInterestRepository _interestRepository;
    private DeleteInterestCommandHandler _handler;
    
    [SetUp]
    public void Setup()
    {
        _dbContext = PlugGenerator.DbContext();

        _interestRepository = new InterestRepository(_dbContext);
    }

    [Test]
    public void Handle_DeletesSuccessfully_OnAdminRole_OnInterestExists()
    {
        UpdateHandler(true);
        var cancellationToken = new CancellationToken();
        _interestRepository.Create(new Interest(1, "aboba"), cancellationToken);
        var result = _handler.Handle(
            new DeleteInterestCommand(1),
            cancellationToken);

        var isSuccess = result.WaitAsync(cancellationToken).Result.IsSuccess;  
        isSuccess.Should().BeTrue();
    }

    [Test]
    public void Handle_Fails_OnUserRole_OnInterestExists()
    {
        UpdateHandler();
        var cancellationToken = new CancellationToken();
        _interestRepository.Create(new Interest(1, "aboba"), cancellationToken);

        var result = _handler.Handle(
            new DeleteInterestCommand(1),
            cancellationToken);

        var isFailed = result.WaitAsync(cancellationToken).Result.IsFailed;
        isFailed.Should().BeTrue();
    }

    [Test]
    public void Handle_Fails_OnAdminRole_OnInterestDoesNotExist()
    {
        UpdateHandler(true);
        var cancellationToken = new CancellationToken();
        _interestRepository.Create(new Interest(2, "aboba"), cancellationToken);
        var result = _handler.Handle(
            new DeleteInterestCommand(1),
            cancellationToken).WaitAsync(cancellationToken);
        
        var isFailed = result.WaitAsync(cancellationToken).Result.IsFailed;
        isFailed.Should().BeTrue();
    }

    private void UpdateHandler(bool isAdmin = false)
    {
        var currentUser = PlugGenerator.FakeCurrentUser(isAdmin);
        _handler = new DeleteInterestCommandHandler(currentUser, _interestRepository);
    }
}
