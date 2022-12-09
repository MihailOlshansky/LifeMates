using FluentAssertions;
using LifeMates.Domain.Models.Chats;
using NUnit.Framework;

namespace Tests.LifeMates.Domain.Chats;

public class ChatUserTests
{
    private ChatUser _chatUser;

    [TestCase(true, TestName = "isSeen: false -> true")]
    [TestCase(false, TestName = "isSeen: true -> false")]
    public void Update_ReturnsTrue_WhenIsSeenChanged(bool isSeenNewValue)
    {
        _chatUser = new ChatUser(1, !isSeenNewValue);
        var hasChanged = _chatUser.Update(isSeenNewValue).Value;

        hasChanged.Should().BeTrue();
    }
    
    [TestCase(true, TestName = "isSeen: true -> true")]
    [TestCase(false, TestName = "isSeen: false -> false")]
    public void Update_ReturnsFalse_WhenIsSeenDidNotChange(bool isSeenNewValue)
    {
        _chatUser = new ChatUser(1, isSeenNewValue);
        var hasChanged = _chatUser.Update(isSeenNewValue).Value;

        hasChanged.Should().BeFalse();
    }
}
