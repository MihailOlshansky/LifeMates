using FluentAssertions;
using LifeMates.Domain.Models.Users;
using LifeMates.Domain.Shared.Users;
using NUnit.Framework;

namespace Tests.LifeMates.Domain.Users;

public class UserTests
{
    private User _user;

    [SetUp]
    public void Setup()
    {
        _user = new User("user", null, UserGender.Man, UserStatus.Approved, null, 
            new UserLocation(0, 0), new UserSettings(UserGender.Woman), new List<UserImage>(),
            new List<UserContact>(), new List<UserInterest>());
    }
    
    [TestCaseSource(typeof(UserData), nameof(UserData.UpdateUserTests))]
    public void Update_ReturnsTrue_WhenUserChanged(User newUser)
    {
        var hasChanged = _user.Update(newUser.Name, newUser.Description, newUser.Gender, newUser.Birthday,
            newUser.Settings, newUser.Images, newUser.Contacts, newUser.Interests).Value;

        hasChanged.Should().BeTrue();
    }
    
    [Test]
    public void Update_ReturnsFalse_WhenUserDidNotChange()
    {
        var hasChanged = _user.Update(_user.Name, _user.Description, _user.Gender, _user.Birthday, 
            _user.Settings, _user.Images, _user.Contacts, _user.Interests).Value;

        hasChanged.Should().BeFalse();
    }
    
    [Test]
    public void Update_ReturnsTrue_WhenLocationChanged()
    {
        var hasChanged = _user.Update(new UserLocation(1, 1)).Value;

        hasChanged.Should().BeTrue();
    }
    
    [Test]
    public void Update_ReturnsFalse_WhenLocationDidNotChange()
    {
        var hasChanged = _user.Update(new UserLocation(0, 0)).Value;

        hasChanged.Should().BeFalse();
    }

    [Test]
    public void Update_ReturnsTrue_WhenStatusChanged()
    {
        var hasChanged = _user.Update(UserStatus.Created).Value;

        hasChanged.Should().BeTrue();
    }
    
    [Test]
    public void Update_ReturnsFalse_WhenStatusDidNotChanged()
    {
        var hasChanged = _user.Update(UserStatus.Approved).Value;

        hasChanged.Should().BeFalse();
    }
    
    private class UserData
    {
        public static IEnumerable<TestCaseData> UpdateUserTests
        {
            get
            {
                yield return new TestCaseData(new User("user1", null, UserGender.Man, UserStatus.Approved,
                    null, null, new UserSettings(UserGender.Woman), 
                    new List<UserImage>(), new List<UserContact>(), new List<UserInterest>()));

                yield return new TestCaseData(new User("user", "aboba", UserGender.Man, UserStatus.Approved,
                    null, null, new UserSettings(UserGender.Woman), 
                    new List<UserImage>(), new List<UserContact>(), new List<UserInterest>()));
                
                yield return new TestCaseData(new User("user", null, UserGender.Woman, UserStatus.Approved,
                    null, null, new UserSettings(UserGender.Woman), 
                    new List<UserImage>(), new List<UserContact>(), new List<UserInterest>()));
                
                yield return new TestCaseData(new User("user", null, UserGender.Man, UserStatus.Approved,
                    DateTime.Now, null, new UserSettings(UserGender.Woman), 
                    new List<UserImage>(), new List<UserContact>(), new List<UserInterest>()));
                
                yield return new TestCaseData(new User("user", null, UserGender.Man, UserStatus.Approved,
                    null, null, new UserSettings(UserGender.Man), 
                    new List<UserImage>(), new List<UserContact>(), new List<UserInterest>()));
                
                yield return new TestCaseData(new User("user", null, UserGender.Man, UserStatus.Approved,
                    null, null, new UserSettings(UserGender.Woman),
                    new List<UserImage>() { new UserImage(1, 1, "aboba.com") }, 
                    new List<UserContact>(), new List<UserInterest>()));
                
                yield return new TestCaseData(new User("user", null, UserGender.Man, UserStatus.Approved,
                    null, null, new UserSettings(UserGender.Woman), new List<UserImage>(),
                    new List<UserContact>() { new UserContact(ContactType.Telegram, "@user") },
                    new List<UserInterest>()));
                
                yield return new TestCaseData(new User("user", null, UserGender.Man, UserStatus.Approved,
                    null, null, new UserSettings(UserGender.Woman),
                    new List<UserImage>(), new List<UserContact>(),
                    new List<UserInterest>(){new UserInterest(1, 1, 1)}));
            }
        }
    }
}
