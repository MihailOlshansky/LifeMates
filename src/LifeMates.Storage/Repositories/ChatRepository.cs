using LifeMates.Domain.Models.Chats;
using LifeMates.Storage.SharedKernel.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeMates.Storage.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly LifematesDbContext _dbContext;

    public ChatRepository(LifematesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Create(Chat chat)
    {
        _dbContext.Chats.Add(chat);
    }

    public async Task<Chat?> Get(long chatId, CancellationToken cancellationToken)
    {
        return await _dbContext.Chats.Include(x => x.ChatUsers).FirstOrDefaultAsync(x => x.Id == chatId, cancellationToken);
    }

    public Task<bool> IsUserChat(long chatId, long userId, CancellationToken cancellationToken)
    {
        return _dbContext.ChatUsers.AnyAsync(x => x.ChatId == chatId && x.UserId == userId, cancellationToken);
    }
}