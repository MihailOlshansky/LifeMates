using AutoMapper;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Domain.Shared.Users;
using LifeMates.Storage;
using LifeMates.Storage.Profiles;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.LifeMates.Core;

public class PlugGenerator
{ 
    public static ICurrentUser FakeCurrentUser(bool isAdmin = false)
    {
        var currentUser = new Mock<ICurrentUser>();
        currentUser.Setup(x => x.GetUserId()).Returns(1);
        currentUser.Setup(x => x.IsInRole(RoleTypes.Admin)).Returns(isAdmin);
        currentUser.Setup(x => x.GetUserCredentialsId()).Returns("aboba");
        return currentUser.Object;   
    }

    public static IMapper EntityToDomainMapper()
    {
        return new Mapper(new MapperConfiguration(expression => expression.AddProfile(new ViewProfile())));
    }

    public static LifematesDbContext DbContext()
    {
        var dbContext = new LifematesDbContext(new DbContextOptionsBuilder<LifematesDbContext>()
            .UseInMemoryDatabase(databaseName: "lifemates")
            .Options);
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        return dbContext;
    }
    
}