using LifeMates.Domain.Models.Users;
using LifeMates.Storage.SharedKernel.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeMates.Storage.Repositories;

public class UserRepository : IUserRepository
{
    private readonly LifematesDbContext _dbContext;

    public UserRepository(LifematesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task Commit(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Create(User user, CancellationToken cancellationToken)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<User?> Get(long id, CancellationToken cancellationToken)
    {
        return _dbContext.Users
            .Include(x => x.Contacts)
            .Include(x => x.Settings)
            .Include(x => x.Location)
            .Include(x => x.Images)
            .Include(x => x.Interests)
            .Include(x => x.Credentials)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<bool> Exists(long id, CancellationToken cancellationToken)
    {
        return _dbContext.Users.AnyAsync(x => x.Id == id, cancellationToken);
    }
}