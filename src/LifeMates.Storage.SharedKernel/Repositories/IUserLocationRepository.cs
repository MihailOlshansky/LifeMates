using LifeMates.Domain.Models.Users;

namespace LifeMates.Storage.SharedKernel.Repositories;

public interface IUserLocationRepository
{
    Task<UserLocation?> GetByUserId(long userId, CancellationToken cancellationToken);
}