using FluentAssertions;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Core.Commands.Interest;
using LifeMates.Core.Commands.User;
using LifeMates.Domain.Models.Users;
using LifeMates.Domain.Shared.Users;
using LifeMates.Storage;
using LifeMates.Storage.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Tests.LifeMates.Core.Commands.User;

public class DeleteMeCommandTests
{
    private LifematesDbContext _dbContext;
    private DeleteMeCommandHandler _handler;
    private UserRepository _userRepository;
    private ICurrentUser _currentUser;
    
    [SetUp]
    public void Setup()
    {
        _dbContext = new LifematesDbContext(new DbContextOptionsBuilder<LifematesDbContext>()
            .UseInMemoryDatabase(databaseName: "lifemates")
            .Options);
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        _userRepository = new UserRepository(_dbContext);
        _currentUser = PlugGenerator.FakeCurrentUser();
        _handler = new DeleteMeCommandHandler(_currentUser, _userRepository);
    }

    [Test]
    public async Task Handle_SetsDeletedStatus_WhenUserExists()
    {
        var cancellationToken = new CancellationToken();
        await _userRepository.Create(CreateUserFromFakeCurrentUser(), cancellationToken);
        
        await _handler.Handle(
            new DeleteMeCommand(),
            cancellationToken);
        
        var userStatus = (await _userRepository.Get(_currentUser.GetUserId(), cancellationToken))?.Status;  
        
        userStatus.Should().Be(UserStatus.Deleted);
    }

    [Test]
    public void Handle_Fails_WhenUserDoesNotExist()
    {
        var cancellationToken = new CancellationToken();
        
        var isDeleted = _handler.Handle(
            new DeleteMeCommand(),
            cancellationToken).WaitAsync(cancellationToken).Result.IsSuccess;

        isDeleted.Should().BeFalse();
    }
    
    private global::LifeMates.Domain.Models.Users.User CreateUserFromFakeCurrentUser()
    {
        return new global::LifeMates.Domain.Models.Users.User(_currentUser.GetUserId(), "aboba", null, UserGender.Man,
            UserStatus.Approved, null, null,
            new UserSettings(UserGender.Woman), null, null, null);
    } 

}