using LifeMates.Domain.Models.Users;
using LifeMates.Storage.SharedKernel.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeMates.Storage.Repositories;

public class UserSettingsRepository : IUserSettingsRepository
{
    private readonly LifematesDbContext _dbContext;

    public UserSettingsRepository(LifematesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<UserSettings?> GetByUserId(long userId, CancellationToken cancellationToken)
    {
        return _dbContext.UserSettings.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
    }
}