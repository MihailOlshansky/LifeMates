using LifeMates.Domain.Shared.Users;

namespace LifeMates.Domain.ReadOnly.Users;

public class UserView
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public UserGender Gender { get; set; }
    public UserStatus Status { get; set; }
    public DateTime? Birthday { get; set; }
    public UserLocationView? Location { get; set; }
    public ICollection<UserInterestView> Interests { get; set; } = new List<UserInterestView>();
    public ICollection<string> ImagesUrls { get; set; } = new List<string>();
    public ICollection<UserContactView> Contacts { get; set; } = new List<UserContactView>();
}