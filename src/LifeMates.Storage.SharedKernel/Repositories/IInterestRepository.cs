using LifeMates.Domain.Models.Interests;

namespace LifeMates.Storage.SharedKernel.Repositories;

public interface IInterestRepository
{
    Task Create(Interest interest, CancellationToken cancellationToken);
    
    Task<Interest?> GetById(long id, CancellationToken cancellationToken);

    Task<bool> Exists(string value, CancellationToken cancellationToken);
    
    Task<bool> Exists(long id, CancellationToken cancellationToken);
    
    Task<bool> Exists(IEnumerable<long> id, CancellationToken cancellationToken);

    Task Remove(Interest interest, CancellationToken cancellationToken);
}