using AutoMapper;
using LifeMates.Domain.ReadOnly.Chats;
using LifeMates.Storage.SharedKernel.ReadOnlyRepositories;
using Microsoft.EntityFrameworkCore;

namespace LifeMates.Storage.ReadOnlyRepositories;

public class ChatReadOnlyRepository : IChatReadOnlyRepository
{
    private readonly IMapper _mapper;
    private readonly LifematesDbContext _dbContext;

    public ChatReadOnlyRepository(
        IMapper mapper,
        LifematesDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<ICollection<MatchView>> GetMatches(long userId, int offset, int limit, CancellationToken cancellationToken)
    {
        var chats = (await _dbContext.Chats
            .AsNoTracking()
            .Include(c => c.ChatUsers)
            .ThenInclude(cu => cu.User)
            .ThenInclude(u => u.Images)
            .Include(c => c.ChatUsers)
            .ThenInclude(cu => cu.User)
            .ThenInclude(u => u.Contacts)
            .Where(chat => chat.ChatUsers.Any(cu => cu.UserId == userId))
            .OrderByDescending(x => x.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken));
        
        var chatViews = chats
            .Select(c => _mapper.Map<MatchView>(c.ChatUsers.First(cu => cu.UserId != userId)))
            .ToList();

        return chatViews;
    }

    public Task<int> CountMatches(long userId, CancellationToken cancellationToken)
    {
        return _dbContext.ChatUsers.CountAsync(x => x.UserId == userId, cancellationToken);
    }

    public Task<int> CountChats(long userId, CancellationToken cancellationToken)
    {
        return _dbContext.Messages
            .Where(x => x.UserId == userId)
            .GroupBy(x => x.ChatId)
            .CountAsync(cancellationToken);
    }

    public async Task<ICollection<ChatView>> GetChats(long userId, int offset, int limit, CancellationToken cancellationToken)
    {
        var messagesIds = await _dbContext
            .ChatUsers
            .Where(x => x.UserId == userId)
            .Include(x => x.Chat)
            .ThenInclude(x => x.Messages)
            .Select(x => x.Chat.Messages.OrderByDescending(m => m.CreatedAt).First())
            .Where(x => x != null)
            .OrderByDescending(x => x.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        var messages = (await _dbContext
            .Messages
            .Where(x => messagesIds.Any(id => id == x.Id))
            .Include(x => x.User)
            .Include(x => x.Chat)
            .ThenInclude(x => x.ChatUsers)
            .ToListAsync(cancellationToken))
            .ToDictionary(x => x.Id, x => x);

        var chatViews = messagesIds.Select(x => _mapper.Map<ChatView>(messages[x])).ToList();

        return chatViews;
    }
}