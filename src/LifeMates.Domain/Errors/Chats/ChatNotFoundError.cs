using LifeMates.Domain.Models.Chats;

namespace LifeMates.Domain.Errors.Chats;

public class ChatNotFoundError : ApplicationError
{
    public ChatNotFoundError(long chatId)
    {
        Message = $"Чат с id = {chatId} не найден";
        PropertyName = nameof(Chat.Id);
    }
}