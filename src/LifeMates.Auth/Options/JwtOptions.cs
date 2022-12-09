using static System.String;

namespace LifeMates.Auth.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    
    public string Issuer { get; set; } = Empty;
    public string Audience { get; set; } = Empty;
    public string AccessTokenSecret { get; set; } = Empty;
    public string RefreshTokenSecret { get; set; } = Empty;
    public int TokenValidityInHours { get; set; }
    public int RefreshTokenValidityInDays { get; set; }
}