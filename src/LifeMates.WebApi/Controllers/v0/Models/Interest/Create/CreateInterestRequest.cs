using System.ComponentModel.DataAnnotations;

namespace LifeMates.WebApi.Controllers.v0.Models.Interest.Create;

public class CreateInterestRequest
{
    [Required]
    public string Value { get; set; } = string.Empty;
}