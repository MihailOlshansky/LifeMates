using FluentAssertions;
using LifeMates.Domain.Models.Chats;
using NUnit.Framework;

namespace Tests.LifeMates.Domain.Chats;

public class MessageTests
{
    private Message _message;

    [TestCase(true, TestName = "isSeen: false -> true")]
    [TestCase(false, TestName = "isSeen: true -> false")]
    public void Update_ReturnsTrue_WhenIsSeenChanged(bool isSeenNewValue)
    {
        _message = new Message(1, 1, "aboba");
        _message.Update(!isSeenNewValue);
        var hasChanged = _message.Update(isSeenNewValue).Value;

        hasChanged.Should().BeTrue();
    }
    
    [TestCase(true, TestName = "isSeen: true -> true")]
    [TestCase(false, TestName = "isSeen: false -> false")]
    public void Update_ReturnsFalse_WhenIsSeenDidNotChange(bool isSeenNewValue)
    {
        _message = new Message(1, 1, "aboba");
        _message.Update(isSeenNewValue);
        var hasChanged = _message.Update(isSeenNewValue).Value;

        hasChanged.Should().BeFalse();
    }
}