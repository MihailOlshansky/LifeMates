using System.ComponentModel.DataAnnotations;

namespace LifeMates.WebApi.Controllers.v0.Models.User.Search;

public class SearchResponse
{
    [Required]
    public ICollection<UserView> Mates { get; set; } = new List<UserView>();
}