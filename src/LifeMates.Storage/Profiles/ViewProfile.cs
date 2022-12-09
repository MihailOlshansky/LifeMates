using AutoMapper;
using LifeMates.Domain.ReadOnly.Interests;
using LifeMates.Domain.ReadOnly.Chats;
using LifeMates.Domain.ReadOnly.Users;

namespace LifeMates.Storage.Profiles;

public class ViewProfile : Profile
{
    public ViewProfile()
    {
        CreateMap<Domain.Models.Users.UserLocation, UserLocationView>();
        CreateMap<Domain.Models.Users.UserContact, UserContactView>();
        CreateMap<Domain.Models.Users.UserSettings, UserSettingsView>();
        CreateMap<Domain.Models.Users.UserInterest, UserInterestView>()
            .ForMember(d => d.Id, m => m.MapFrom(s => s.InterestId))
            .ForMember(d => d.Value, m => m.MapFrom(s => s.Interest!.Value));
        
        UserViewMap();
        InterestViewMap();
        ChatViewMap();
    }

    private void UserViewMap()
    {
        CreateMap<Domain.Models.Users.User, MeView>()
            .ForMember(d => d.Email, m => m.MapFrom(s => s.Credentials!.Email))
            .ForMember(d => d.Interests, m => m.MapFrom(s => s.Interests))
            .ForMember(d => d.ImagesUrls, m => m.Ignore())
            .AfterMap((s, d) =>
            {
                d.ImagesUrls = s.Images!.Select(i => i.Url).ToList();
            });
        
        CreateMap<Domain.Models.Users.User, UserView>()
            .ForMember(d => d.Interests, m => m.MapFrom(s => s.Interests))
            .ForMember(d => d.ImagesUrls, m => m.Ignore())
            .AfterMap((s, d) =>
            {
                d.ImagesUrls = s.Images!.Select(i => i.Url).ToList();
            });
    }

    private void InterestViewMap()
    {
        CreateMap<Domain.Models.Interests.Interest, InterestView>();
    }

    private void ChatViewMap()
    {
        CreateMap<Domain.Models.Chats.ChatUser, MatchView>()
            .ForMember(d => d.Id, m => m.MapFrom(s => s.ChatId))
            .ForMember(d => d.User, m => m.MapFrom(s => s.User));

        CreateMap<Domain.Models.Chats.ChatUser, UserChatView>()
            .ForMember(d => d.Id, m => m.MapFrom(s => s.User.Id))
            .ForMember(d => d.Name, m => m.MapFrom(s => s.User.Name))
            .ForMember(d => d.ImagesUrls, m => m.Ignore())
            .AfterMap((s, d) =>
            {
                d.ImagesUrls = s.User.Images?.Select(x => x.Url).ToList();
            });
        
        CreateMap<Domain.Models.Chats.Message, ChatView>()
            .ForMember(d => d.Id, m => m.MapFrom(s => s.ChatId))
            .ForMember(d => d.User, m => m.MapFrom(s => s.User))
            .ForMember(d => d.Message, m => m.MapFrom(s => s));
        
        CreateMap<Domain.Models.Chats.Message, ChatMessageView>()
            .ForMember(d => d.User, m => m.MapFrom(s => s.User));

        CreateMap<Domain.Models.Users.User, UserChatView>()
            .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
            .ForMember(d => d.Name, m => m.MapFrom(s => s.Name))
            .ForMember(d => d.ImagesUrls, m => m.Ignore())
            .AfterMap((s, d) =>
            {
                d.ImagesUrls = s.Images?.Select(x => x.Url).ToList();
            });
    }
}