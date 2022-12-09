using AutoMapper;
using LifeMates.Core.Commands.Interest;
using LifeMates.Core.Queries.Interest;
using LifeMates.WebApi.Controllers.v0.Models.Interest;
using LifeMates.WebApi.Controllers.v0.Models.Interest.Create;
using LifeMates.WebApi.Controllers.v0.Models.Interest.Get;

namespace LifeMates.WebApi.Profiles;

public class InterestProfile : Profile
{
    public InterestProfile()
    {
        GetInterestMap();
        CreateInterestMap();
    }
    
    private void GetInterestMap()
    {
        CreateMap<Domain.ReadOnly.Interests.InterestView, InterestView>();
        CreateMap<GetInterestsQueryResponse, GetInterestsResponse>()
            .ForMember(d => d.Interests, m => m.MapFrom(s => s.InterestViews));
    }

    private void CreateInterestMap()
    {
        CreateMap<CreateInterestRequest, CreateInterestCommand>();
    }
}