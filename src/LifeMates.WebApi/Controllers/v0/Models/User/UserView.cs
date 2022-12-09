using System.ComponentModel.DataAnnotations;
using LifeMates.WebApi.Controllers.v0.Models.User.Details;
using LifeMates.WebApi.Controllers.v0.Models.User.Details.Contact;

namespace LifeMates.WebApi.Controllers.v0.Models.User;

public class UserView
{
    [Required]
    public long Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public UserGender Gender { get; set; }
    [Required]
    public UserStatus Status { get; set; }
    public int? Age { get; set; } 
    public int? Distance { get; set; }
    [Required]
    public ICollection<string> Interests { get; set; } = new List<string>();
    [Required]
    public ICollection<string> ImagesUrls { get; set; } = new List<string>();
    [Required]
    public ICollection<UserContactView> Contacts { get; set; } = new List<UserContactView>();
}