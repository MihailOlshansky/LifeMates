using LifeMates.Domain.Models.Users;
using LifeMates.Domain.ReadOnly.Users;
using LifeMates.Domain.Shared.Users;

namespace LifeMates.Storage.SharedKernel.ReadOnlyRepositories;

public interface IUserReadOnlyRepository
{
    Task<MeView?> GetMe(long id, CancellationToken cancellationToken);
    Task<UserView?> Get(long id, CancellationToken cancellationToken);
    Task<ICollection<UserView>> Search(SearchFilter filter, CancellationToken cancellationToken);
}

public class SearchFilter
{
    public long SearchForUserId { get; set; }
    public int Limit { get; set; }
    public int Offset { get; set; }
    public double Radius { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public UserGender SearchingGender { get; set; }
}