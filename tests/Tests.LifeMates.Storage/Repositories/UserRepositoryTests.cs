using LifeMates.Domain.Models.Users;
using LifeMates.Domain.Shared.Users;
using LifeMates.Storage.Repositories;
using FluentAssertions;
using NUnit.Framework;
using Tests.LifeMates.Core;

namespace Tests.LifeMates.Storage.Repositories;

public class UserRepositoryTests
{
    private UserRepository _userRepository;

    [SetUp]
    public void SetUp()
    {
        var dbContext = PlugGenerator.DbContext();
        
        _userRepository = new UserRepository(dbContext);
    }

    [TestCaseSource(typeof(UserData), nameof(UserData.CreateUserTests))]
    public void Create_CompletesSuccessfully_OnValidParameters(User user)
    {
        var cancellationToken = new CancellationToken();

        var result = _userRepository.Create(user, cancellationToken);
        var isCreated = result.WaitAsync(cancellationToken).IsCompletedSuccessfully;
        
        isCreated.Should().BeTrue();
    }

    [Test]
    public void Create_Fails_OnDoubleCreation()
    {
        var user = new User("aboba", null, UserGender.Man, UserStatus.Approved, null, null,
            new UserSettings(UserGender.Woman), null, null, null);
        var cancellationToken = new CancellationToken();

        _userRepository.Create(user, cancellationToken).WaitAsync(cancellationToken);
        var result = _userRepository.Create(user, cancellationToken);
        var isFailed = result.WaitAsync(cancellationToken).IsFaulted;
        
        isFailed.Should().BeTrue();
    }

    [Test]
    public async Task Get_ReturnsRightUser_WhenUserExists()
    {
        var user = new User(1, "aboba", null, UserGender.Man, UserStatus.Approved, null, null,
            new UserSettings(UserGender.Woman), null, null, null);
        var cancellationToken = new CancellationToken();

        await _userRepository.Create(user, cancellationToken);
        var bdUser = await _userRepository.Get(1, new CancellationToken());

        bdUser.Should().BeEquivalentTo(user);
    }

    [Test]
    public async Task Get_ReturnsNull_WhenUserDoesNotExist()
    {
        var bdUser = await _userRepository.Get(1, new CancellationToken());

        bdUser.Should().BeNull();
    }

    [Test]
    public async Task Exists_ReturnsTrue_WhenUserExists()
    {
        var user = new User(1, "aboba", null, UserGender.Man, UserStatus.Approved, null, null,
            new UserSettings(UserGender.Woman), null, null, null);
        var cancellationToken = new CancellationToken();

        await _userRepository.Create(user, cancellationToken);
        var isUserExists = await _userRepository.Exists(1, new CancellationToken());

        isUserExists.Should().BeTrue();
    }

    [Test]
    public async Task Exists_ReturnsFalse_WhenUserDoesNotExist()
    {
        var isUserExists = await _userRepository.Exists(1, new CancellationToken());

        isUserExists.Should().BeFalse();
    }
 
    private class UserData
    {
        public static IEnumerable<TestCaseData> CreateUserTests
        {
            get
            {
                yield return new TestCaseData(new User(
                        "User1",
                        "First User",
                        UserGender.Man,
                        UserStatus.Approved,
                        DateTime.Now,
                        new UserLocation(10.123, 11.32),
                        new UserSettings(UserGender.Woman),
                        new List<UserImage>() { new UserImage(1, 1, "https://user1.com/image") },
                        new List<UserContact>() { new UserContact(ContactType.Telegram, "@user1") },
                        new List<UserInterest>() { new UserInterest(1, 1, 1) }))
                    .SetName("With all parameters");

                yield return new TestCaseData(new User(
                        "User2",
                        null,
                        UserGender.Man,
                        UserStatus.Approved,
                        null,
                        null,
                        new UserSettings(UserGender.Woman),
                        null,
                        null,
                        null))
                    .SetName("With not nullable parameters");
            }
        }
    }
}