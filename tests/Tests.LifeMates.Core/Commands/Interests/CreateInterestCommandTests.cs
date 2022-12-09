using FluentAssertions;
using LifeMates.Core.Commands.Interest;
using LifeMates.Storage;
using LifeMates.Storage.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Tests.LifeMates.Core.Commands.Interests;

public class CreateInterestCommandUnitTests
{
    private LifematesDbContext _dbContext;
    private CreateInterestCommandHandler _handler;
    
    [SetUp]
    public void Setup()
    {
        _dbContext = PlugGenerator.DbContext();
    }

    [Test]
    public void Handle_CreatesSuccessfully_OnAdminRoleWithValidParameters()
    {
        UpdateHandler(true);
        var result = _handler.Handle(
            new CreateInterestCommand("aboba"),
            CancellationToken.None);

        var isSuccess = result.WaitAsync(CancellationToken.None).Result.IsSuccess;  
        
        isSuccess.Should().BeTrue();
    }

    [Test]
    public void Handle_Fails_OnUserRoleWithValidParameters()
    {
        UpdateHandler();
        var result = _handler.Handle(
            new CreateInterestCommand("aboba"),
            CancellationToken.None);

        var isFailed = result.WaitAsync(CancellationToken.None).Result.IsFailed;
        isFailed.Should().BeTrue();
    }

    [Test]
    public async Task Handle_Fails_OnAdminRoleWithDuplicatedInterest()
    {
        UpdateHandler(true);
        await _handler.Handle(new CreateInterestCommand("aboba"), CancellationToken.None);
        var result = await _handler.Handle(new CreateInterestCommand("aboba"), CancellationToken.None);
        
        var isFailed = result.IsFailed;
        isFailed.Should().BeTrue();
    }

    private void UpdateHandler(bool isAdmin = false)
    {
        var interestRepository = new InterestRepository(_dbContext);
        var currentUser = PlugGenerator.FakeCurrentUser(isAdmin);

        _handler = new CreateInterestCommandHandler(currentUser, interestRepository);
    }
}