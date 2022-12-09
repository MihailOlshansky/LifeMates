namespace LifeMates.WebApi.Controllers.v0.Models.User.Details.Contact;

public class UserContactView
{
    public long Id { get; set; }
    public ContactType Type { get; set; }
    public string Value { get; set; } = string.Empty;
}