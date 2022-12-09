namespace LifeMates.WebApi.Controllers.v0.Models.Interest.Get;

public class GetInterestsResponse
{
    public ICollection<InterestView> Interests { get; set; } = new List<InterestView>();
}