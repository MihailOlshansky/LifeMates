using LifeMates.Domain.Models.Chats;
using LifeMates.Storage.SharedKernel.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeMates.Storage.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly LifematesDbContext _lifematesDbContext;

    public MessageRepository(LifematesDbContext lifematesDbContext)
    {
        _lifematesDbContext = lifematesDbContext;
    }

    public async Task<ICollection<Message>> Get(long chatId, int limit, int offset, CancellationToken cancellationToken)
    {
        return await _lifematesDbContext.Messages
            .Where(x => chatId == x.ChatId)
            .OrderByDescending(x => x.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .Include(x => x.Chat)
            .Include(x => x.User)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> Count(long chatId, CancellationToken cancellationToken)
    {
        return await _lifematesDbContext.Messages
            .Where(x => chatId == x.ChatId)
            .CountAsync(cancellationToken);
    }
}