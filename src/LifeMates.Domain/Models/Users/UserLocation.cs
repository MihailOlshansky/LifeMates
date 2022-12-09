using FluentResults;

namespace LifeMates.Domain.Models.Users;

public class UserLocation
{
    private const double Tolerance = 1e-3;
    
    public long Id { get; private set; }
    public long UserId { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    
    public User? User { get; set; }
    
    public UserLocation(long id, long userId, double latitude, double longitude)
    {
        Id = id;
        UserId = userId;
        Latitude = latitude;
        Longitude = longitude;
    }

    public UserLocation(double latitude, double longitude) : this(default, default, latitude, longitude) { }

    public Result<bool> Update(UserLocation location)
    {
        if (Math.Abs(Latitude - location.Latitude) < Tolerance 
            && Math.Abs(Longitude - location.Longitude) < Tolerance)
        {
            return Result.Ok(false);
        }
        
        Latitude = location.Latitude;
        Longitude = location.Longitude;

        return Result.Ok(true);
    }
    
    protected UserLocation()
    {
        
    }
}