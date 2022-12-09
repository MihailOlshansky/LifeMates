using System.Device.Location;
using AutoMapper;
using LifeMates.Core.Commands.Match;
using LifeMates.Core.Commands.User;
using LifeMates.Core.Queries.Users;
using LifeMates.WebApi.Controllers.v0.Models.User;
using LifeMates.WebApi.Controllers.v0.Models.User.Details;
using LifeMates.WebApi.Controllers.v0.Models.User.Details.Contact;
using LifeMates.WebApi.Controllers.v0.Models.User.Edit;
using LifeMates.WebApi.Controllers.v0.Models.User.Likes;
using LifeMates.WebApi.Controllers.v0.Models.User.Login;
using LifeMates.WebApi.Controllers.v0.Models.User.Register;
using LifeMates.WebApi.Controllers.v0.Models.User.Search;
using UserContact = LifeMates.Core.Commands.User.Models.UserContact;
using UserLocation = LifeMates.Core.Commands.User.Models.UserLocation;
using UserSettings = LifeMates.Core.Commands.User.Models.UserSettings;

namespace LifeMates.WebApi.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        DomainMap();
        
        RegisterMap();
        LoginMap();
        SearchMap();
        GetMap();
        EditMap();
        LikeMap();
    }

    private void LikeMap()
    {
        CreateMap<LikeCommandResponse, LikeResponse>();
    }

    private void DomainMap()
    {
        CreateMap<Domain.Shared.Users.ContactType, ContactType>();
        CreateMap<Domain.Shared.Users.UserGender, UserGender>();
        CreateMap<Domain.Shared.Users.UserStatus, UserStatus>();
        CreateMap<Domain.ReadOnly.Users.UserLocationView, Controllers.v0.Models.User.Details.UserLocation>();
        CreateMap<Domain.ReadOnly.Users.UserSettingsView, Controllers.v0.Models.User.Details.UserSettings>();
        CreateMap<Domain.ReadOnly.Users.UserContactView, UserContactView>();
        CreateMap<Domain.ReadOnly.Users.UserInterestView, UserInterest>();
        CreateMap<Domain.ReadOnly.Users.MeView, ProfileView>();
        CreateMap<Domain.ReadOnly.Users.UserView, UserView>()
            .ForMember(d => d.Age, m => m.Ignore())
            .ForMember(d => d.Distance, m => m.Ignore())
            .AfterMap((s, d) =>
            {
                d.Age = s.Birthday is null
                    ? null
                    : DateTime.UtcNow.Subtract(s.Birthday.Value).Days / 365;
            });
    }

    private void RegisterMap()
    {
        CreateMap<RegisterUserRequest, CreateUserCommand>();
        CreateMap<Controllers.v0.Models.User.Details.Contact.UserContact, UserContact>();
        CreateMap<Controllers.v0.Models.User.Details.UserSettings, UserSettings>();
        CreateMap<Controllers.v0.Models.User.Details.UserLocation, UserLocation>();
    }
    
    private void LoginMap()
    {
        CreateMap<LoginRequest, LoginCommand>();
        CreateMap<LoginCommandResponse, LoginResponse>();
        CreateMap<CreateUserCommandResponse, LoginResponse>();
    }
    
    private void SearchMap()
    {
        CreateMap<SearchUserQueryResponse, SearchResponse>()
            .ForMember(d => d.Mates, m => m.MapFrom(s => s.UserViews))
            .AfterMap((s, d) =>
            {
                var pairs = s.UserViews.Select(x => (x, d.Mates.First(m => m.Id == x.Id))).ToList();

                foreach (var (sourceUser, destinationUser) in pairs)
                {
                    if (s.CurrentUserLocation is null || sourceUser.Location is null)
                    {
                        continue;
                    }
                    
                    var currentGeoCoord =
                        new GeoCoordinate(s.CurrentUserLocation.Latitude, s.CurrentUserLocation.Longitude);
                    
                    var userGeoCoord =
                        new GeoCoordinate(sourceUser.Location.Latitude, sourceUser.Location.Longitude);

                    destinationUser.Distance = (int)Math.Ceiling(currentGeoCoord.GetDistanceTo(userGeoCoord) / 1000);
                }
            });
        CreateMap<SearchRequest, SearchUserQuery>();
    }

    private void GetMap()
    {
        CreateMap<long, GetMeQuery>();
        CreateMap<GetUserQueryResponse, UserView>()
            .ForMember(d => d.Id, m => m.MapFrom(s => s.UserView.Id))
            .ForMember(d => d.Name, m => m.MapFrom(s => s.UserView.Name))
            .ForMember(d => d.Description, m => m.MapFrom(s => s.UserView.Description))
            .ForMember(d => d.Gender, m => m.MapFrom(s => s.UserView.Gender))
            .ForMember(d => d.Status, m => m.MapFrom(s => s.UserView.Status))
            .ForMember(d => d.Interests, m => m.MapFrom(s => s.UserView.Interests))
            .ForMember(d => d.ImagesUrls, m => m.MapFrom(s => s.UserView.ImagesUrls))
            .ForMember(d => d.Contacts, m => m.MapFrom(s => s.UserView.Contacts))
            .ForMember(d => d.Age, m => m.Ignore())
            .ForMember(d => d.Distance, m => m.Ignore())
            .AfterMap((s, d) =>
            {
                d.Age = s.UserView.Birthday is null
                    ? null
                    : DateTime.UtcNow.Subtract(s.UserView.Birthday.Value).Days / 365; // костыль, а как лучше?

                if (s.CurrentUserLocation is not null && s.UserView.Location is not null)
                {
                    var currentGeoCoord =
                        new GeoCoordinate(s.CurrentUserLocation.Latitude, s.CurrentUserLocation.Longitude);
                    
                    var userGeoCoord =
                        new GeoCoordinate(s.UserView.Location.Latitude, s.UserView.Location.Longitude);

                    d.Distance = (int)Math.Round(currentGeoCoord.GetDistanceTo(userGeoCoord) / 1000);
                }
            });
    }
    
    private void EditMap()
    {
        CreateMap<EditUserRequest, EditUserCommand>();
        CreateMap<EditUserLocationRequest, EditUserLocationCommand>();
    }
}