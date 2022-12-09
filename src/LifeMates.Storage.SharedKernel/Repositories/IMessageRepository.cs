using LifeMates.Domain.Models.Chats;

namespace LifeMates.Storage.SharedKernel.Repositories;

public interface IMessageRepository
{
    Task<ICollection<Message>> Get(long chatId, int limit, int offset, CancellationToken cancellationToken);
    Task<int> Count(long chatId, CancellationToken cancellationToken);
}