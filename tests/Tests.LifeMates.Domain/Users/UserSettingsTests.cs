using FluentAssertions;
using LifeMates.Domain.Models.Users;
using LifeMates.Domain.Shared.Users;
using NUnit.Framework;

namespace Tests.LifeMates.Domain.Users;

public class UserSettingsTests
{
    private UserSettings _settings;

    [SetUp]
    public void Setup()
    {
        _settings = new UserSettings(UserGender.Man);
    }
    
    [Test]
    public void Update_ReturnsTrue_WhenSettingsChanged()
    {
        var hasChanged = _settings.Update(new UserSettings(UserGender.Woman)).Value;

        hasChanged.Should().BeTrue();
    }
    
    [Test]
    public void Update_ReturnsFalse_WhenSettingsDidNotChange()
    {
        var hasChanged = _settings.Update(new UserSettings(UserGender.Man)).Value;

        hasChanged.Should().BeFalse();
    }

}
