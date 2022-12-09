using LifeMates.Domain.Shared.Users;

namespace LifeMates.Domain.ReadOnly.Users;

public class MeView
{
    public long Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public UserGender Gender { get; set; }
    public string? Birthday { get; set; }
    public UserStatus Status { get; set; }
    public UserSettingsView Settings { get; set; } = default!;
    public ICollection<UserInterestView> Interests { get; set; } = new List<UserInterestView>();
    public ICollection<string> ImagesUrls { get; set; } = new List<string>();
    public ICollection<UserContactView> Contacts { get; set; } = new List<UserContactView>();
}