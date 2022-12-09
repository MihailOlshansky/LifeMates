using LifeMates.Domain.Models.Users;

namespace LifeMates.Storage.SharedKernel.Repositories;

public interface IUserSettingsRepository
{
    Task<UserSettings?> GetByUserId(long userId, CancellationToken cancellationToken);
}