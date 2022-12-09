using FluentAssertions;
using LifeMates.Domain.Models.Users;
using NUnit.Framework;

namespace Tests.LifeMates.Domain.Users;

public class UserLocationTests
{
    private UserLocation _location;

    [SetUp]
    public void Setup()
    {
        _location = new UserLocation(0, 0);
    }

    [TestCase(0, 10, TestName = "longitude changing")]
    [TestCase(10, 0, TestName = "latitude changing")]
    [TestCase(10, 10, TestName = "location changing")]
    public void Update_ReturnsTrue_WhenLocationChanged(double latitude, double longitude)
    {
        var hasChanged = _location.Update(new UserLocation(latitude, longitude)).Value;

        hasChanged.Should().BeTrue();
    }

    [Test]
    public void Update_ReturnsFalse_WhenLocationDidNotChange()
    {
        var hasChanged = _location.Update(new UserLocation(0, 0)).Value;

        hasChanged.Should().BeFalse();
    }
}