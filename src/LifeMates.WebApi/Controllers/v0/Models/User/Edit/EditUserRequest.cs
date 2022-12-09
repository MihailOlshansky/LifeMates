using System.ComponentModel.DataAnnotations;
using LifeMates.WebApi.Controllers.v0.Models.User.Details;
using LifeMates.WebApi.Controllers.v0.Models.User.Details.Contact;

namespace LifeMates.WebApi.Controllers.v0.Models.User.Edit;

public class EditUserRequest
{
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
    [Required]
    public UserSettings Settings { get; set; } = default!;
    [Required]
    public ICollection<UserContact> Contacts { get; set; } = new List<UserContact>();
}

public class EditUserLocationRequest
{
    public UserLocation? Location { get; set; }
}