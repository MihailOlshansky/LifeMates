using LifeMates.Domain.Models.Chats;

namespace LifeMates.Storage.SharedKernel.Repositories;

public interface IChatRepository
{
    void Create(Chat chat);
    Task<Chat?> Get(long chatId, CancellationToken cancellationToken);
    Task<bool> IsUserChat(long chatId, long userId, CancellationToken cancellationToken);
}