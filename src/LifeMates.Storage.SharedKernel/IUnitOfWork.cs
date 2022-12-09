namespace LifeMates.Storage.SharedKernel;

public interface IUnitOfWork
{
    Task Commit(CancellationToken cancellationToken);
}