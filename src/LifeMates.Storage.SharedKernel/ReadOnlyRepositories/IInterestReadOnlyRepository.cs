using LifeMates.Domain.ReadOnly.Interests;

namespace LifeMates.Storage.SharedKernel.ReadOnlyRepositories;

public interface IInterestReadOnlyRepository
{
    Task<ICollection<InterestView>> Get(CancellationToken cancellationToken);
}