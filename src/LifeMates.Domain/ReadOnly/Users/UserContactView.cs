using LifeMates.Domain.Shared.Users;

namespace LifeMates.Domain.ReadOnly.Users;

public class UserContactView
{
    public long Id { get; set; }
    public ContactType Type { get; set; }
    public string Value { get; set; } = string.Empty;
}