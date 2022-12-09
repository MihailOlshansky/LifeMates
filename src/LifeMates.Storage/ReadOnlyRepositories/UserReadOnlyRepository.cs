using AutoMapper;
using LifeMates.Domain.ReadOnly.Users;
using LifeMates.Domain.Shared.Users;
using LifeMates.Storage.SharedKernel.ReadOnlyRepositories;
using Microsoft.EntityFrameworkCore;

namespace LifeMates.Storage.ReadOnlyRepositories;

public class UserReadOnlyRepository : IUserReadOnlyRepository
{
    private readonly IMapper _mapper;
    private readonly LifematesDbContext _dbContext;

    public UserReadOnlyRepository(IMapper mapper, LifematesDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<MeView?> GetMe(long id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.AsNoTracking()
            .Include(x => x.Contacts)
            .Include(x => x.Settings)
            .Include(x => x.Images)
            .Include(x => x.Interests)
                !.ThenInclude(x => x.Interest)
            .Include(x => x.Credentials)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return user is null
            ? null            
            : _mapper.Map<MeView>(user);
    }

    public async Task<UserView?> Get(long id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.AsNoTracking()
            .Include(x => x.Contacts)
            .Include(x => x.Location)
            .Include(x => x.Images)
            .Include(x => x.Interests)
                !.ThenInclude(x => x.Interest)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return user is null
            ? null            
            : _mapper.Map<UserView>(user);
    }

    public async Task<ICollection<UserView>> Search(SearchFilter filter, CancellationToken cancellationToken)
    {
        var userId = filter.SearchForUserId;
        var radius = filter.Radius;
        var gender = filter.SearchingGender.ToString();
        var latitude = filter.Latitude;
        var longitude = filter.Longitude;
        var limit = filter.Limit;
        var offset = filter.Offset;
        
        // TODO как быстрее чем в 3 запроса?
        // те, кого еще не смотрели
        var usersIds = (await _dbContext.Users.FromSqlRaw(
                @"SELECT DISTINCT [User].Id
                  FROM [User]
                  LEFT JOIN [UserLocation] on [User].Id = [UserLocation].UserId
                  WHERE [User].Id != {0} AND
		                Gender = {1} AND
                        Status = {5} AND" +
                    (latitude is null || longitude is null
                        ? string.Empty
                        : @"(
                        Latitude IS NULL OR
                        Longitude IS NULL OR
                        ABS(Latitude - {2}) < {4} AND
		                ABS(Longitude - {3}) < {4}      
		            ) AND")
                    + @"[User].Id NOT IN (SELECT LikedUserId FROM [UserLikes] WHERE UserId = {0}) AND
	                [User].Id NOT IN (SELECT DislikedUserId FROM [UserDislikes] WHERE UserId = {0})
                  ORDER BY [User].Id ASC
                  OFFSET {6} ROWS FETCH NEXT {7} ROWS ONLY",
                userId, gender, latitude, longitude, radius, UserStatus.Approved.ToString(), offset, limit)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken));

        var users = (await _dbContext.Users.AsNoTracking()
            .Include(x => x.Contacts)
            .Include(x => x.Location)
            .Include(x => x.Images)
            .Include(x => x.Interests)
            !.ThenInclude(x => x.Interest)
            .Where(x => usersIds.Contains(x.Id))
            .ToListAsync(cancellationToken)).ToDictionary(x => x.Id, x => x);

        // сохраняю порядок
        return usersIds.Select(id => users[id]).Select(_mapper.Map<UserView>).ToList();
    }
}