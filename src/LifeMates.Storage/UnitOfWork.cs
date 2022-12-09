using LifeMates.Storage.SharedKernel;

namespace LifeMates.Storage;

public class UnitOfWork : IUnitOfWork
{
    private readonly LifematesDbContext _dbContext;

    public UnitOfWork(LifematesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task Commit(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}