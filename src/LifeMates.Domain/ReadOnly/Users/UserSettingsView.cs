using LifeMates.Domain.Shared.Users;

namespace LifeMates.Domain.ReadOnly.Users;

public class UserSettingsView
{
    public long Id { get; set; }
    public UserGender ShowingGender { get; set; }
}