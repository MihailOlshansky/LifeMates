using LifeMates.Domain.Models.Chats;

namespace LifeMates.Storage.SharedKernel.Repositories;

public interface IUserChatRepository
{
    Task<ICollection<ChatUser>> Get(long userId, CancellationToken cancellationToken);
    Task<ICollection<ChatUser>> Get(long userId, ICollection<long> chatIds, CancellationToken cancellationToken);
}