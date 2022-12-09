using FluentResults;
using LifeMates.Domain.Models.Interests;
using LifeMates.Storage.SharedKernel.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeMates.Storage.Repositories;

public class InterestRepository : IInterestRepository
{
    private readonly LifematesDbContext _dbContext;

    public InterestRepository(LifematesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Create(Interest interest, CancellationToken cancellationToken)
    {
        _dbContext.Interests.Add(interest);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Interest?> GetById(long id, CancellationToken cancellationToken)
    {
        return _dbContext.Interests.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }
    
    public Task<bool> Exists(string value, CancellationToken cancellationToken)
    {
        return _dbContext.Interests.AnyAsync(x => x.Value == value, cancellationToken);
    }

    public Task<bool> Exists(long id, CancellationToken cancellationToken)
    {
        return _dbContext.Interests.AnyAsync(x => x.Id == id, cancellationToken);
    }
    
    public async Task<bool> Exists(IEnumerable<long> ids, CancellationToken cancellationToken)
    {
        return await _dbContext
            .Interests
            .Where(i => ids.Contains(i.Id))
            .CountAsync(cancellationToken) == ids.Count();
    }

    public async Task Remove(Interest interest, CancellationToken cancellationToken)
    {
        _dbContext.Interests.Remove(interest);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}