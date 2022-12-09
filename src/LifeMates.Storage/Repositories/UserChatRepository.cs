using LifeMates.Domain.Models.Chats;
using LifeMates.Storage.SharedKernel.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeMates.Storage.Repositories;

public class UserChatRepository : IUserChatRepository
{
    private readonly LifematesDbContext _dbContext;

    public UserChatRepository(LifematesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ICollection<ChatUser>> Get(long userId, CancellationToken cancellationToken)
    {
        return await _dbContext.ChatUsers.Where(x => x.UserId == userId).ToListAsync(cancellationToken);
    }

    public async Task<ICollection<ChatUser>> Get(long userId, ICollection<long> chatIds, CancellationToken cancellationToken)
    {
        return await _dbContext.ChatUsers
            .Where(x => x.UserId == userId && chatIds.Contains(x.ChatId))
            .ToListAsync(cancellationToken);
    }
}