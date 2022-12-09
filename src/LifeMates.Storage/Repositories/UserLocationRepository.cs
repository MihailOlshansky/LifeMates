using LifeMates.Domain.Models.Users;
using LifeMates.Storage.SharedKernel.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeMates.Storage.Repositories;

public class UserLocationRepository : IUserLocationRepository
{
    private readonly LifematesDbContext _dbContext;

    public UserLocationRepository(LifematesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<UserLocation?> GetByUserId(long userId, CancellationToken cancellationToken)
    {
        return _dbContext.UserLocations
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
    }
}