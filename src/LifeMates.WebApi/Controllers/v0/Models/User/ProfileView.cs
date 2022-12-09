using System.ComponentModel.DataAnnotations;
using LifeMates.WebApi.Controllers.v0.Models.User.Details;
using LifeMates.WebApi.Controllers.v0.Models.User.Details.Contact;

namespace LifeMates.WebApi.Controllers.v0.Models.User;

public class ProfileView
{
    [Required]
    public long Id { get; set; }
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public UserGender Gender { get; set; }
    public string? Birthday { get; set; }
    [Required]
    public UserSettings Settings { get; set; } = default!;
    [Required]
    public ICollection<UserInterest> Interests { get; set; } = new List<UserInterest>();
    [Required]
    public ICollection<string> ImagesUrls { get; set; } = new List<string>();
    [Required]
    public ICollection<UserContactView> Contacts { get; set; } = new List<UserContactView>();
}