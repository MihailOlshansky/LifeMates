using System.ComponentModel.DataAnnotations;
using LifeMates.WebApi.Controllers.v0.Models.User.Details;
using LifeMates.WebApi.Controllers.v0.Models.User.Details.Contact;

namespace LifeMates.WebApi.Controllers.v0.Models.User.Register;

public class RegisterUserRequest
{
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Required]
    public UserGender Gender { get; set; }
    public DateTime? Birthday { get; set; }
    [Required]
    public ICollection<long> Interests { get; set; } = new List<long>();
    [Required]
    public ICollection<string> ImagesUrls { get; set; } = new List<string>();
    public UserLocation? Location { get; set; }
    [Required]
    public UserSettings Settings { get; set; } = default!;
    [Required]
    public ICollection<UserContact> Contacts { get; set; } = new List<UserContact>();
}