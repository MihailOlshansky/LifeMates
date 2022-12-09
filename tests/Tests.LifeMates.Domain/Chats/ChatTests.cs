using FluentAssertions;
using LifeMates.Domain.Models.Chats;
using NUnit.Framework;

namespace Tests.LifeMates.Domain.Chats;

public class ChatTests
{
    [Test]
    public void IsUserChat_ReturnsTrue_WhenUserInChat()
    {
        var chat = new Chat(new ChatUser(1, true), new ChatUser(2, true));

        var isUserInChat = chat.IsUserChat(1);

        isUserInChat.Should().BeTrue();
    }
    
    [Test]
    public void IsUserChat_ReturnsFalse_WhenUserIsNotInChat()
    {
        var chat = new Chat(new ChatUser(1, true), new ChatUser(2, true));

        var isUserInChat = chat.IsUserChat(3);

        isUserInChat.Should().BeFalse();
    }
}