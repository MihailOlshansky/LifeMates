using System.ComponentModel.DataAnnotations;

namespace LifeMates.WebApi.Controllers.v0.Models.User.Details.Contact;

public class UserContact
{
    [Required]
    public ContactType Type { get; set; }
    [Required]
    public string Value { get; set; } = string.Empty;
}