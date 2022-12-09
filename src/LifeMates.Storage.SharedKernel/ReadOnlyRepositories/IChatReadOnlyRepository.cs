using LifeMates.Domain.ReadOnly.Chats;

namespace LifeMates.Storage.SharedKernel.ReadOnlyRepositories;

public interface IChatReadOnlyRepository
{
    public Task<ICollection<MatchView>> GetMatches(long userId, int offset, int limit, CancellationToken cancellationToken);
    public Task<int> CountMatches(long userId, CancellationToken cancellationToken);
    public Task<int> CountChats(long userId, CancellationToken cancellationToken);
    public Task<ICollection<ChatView>> GetChats(long userId, int offset, int limit, CancellationToken cancellationToken);
}