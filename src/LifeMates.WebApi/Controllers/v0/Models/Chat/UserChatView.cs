using System.ComponentModel.DataAnnotations;

namespace LifeMates.WebApi.Controllers.v0.Models.Chat;

public class UserChatView
{
    [Required]
    public long Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public ICollection<string> ImagesUrls { get; set; } = new List<string>();
}