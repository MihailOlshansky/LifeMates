using AutoMapper;
using LifeMates.Core.Queries.Chat;
using LifeMates.WebApi.Controllers.v0.Models.Chat;
using LifeMates.WebApi.Controllers.v0.Models.Chat.GetChats;
using LifeMates.WebApi.Controllers.v0.Models.Chat.GetMessages;
using LifeMates.WebApi.Controllers.v0.Models.Chat.GetMetches;

namespace LifeMates.WebApi.Profiles;

public class ChatProfile : Profile
{
    public ChatProfile()
    {
        GetMatchesMap();
        GetChatsMap();
        GetMessagesMap();
    }

    private void GetMessagesMap()
    {
        CreateMap<GetMessagesRequest, GetMessagesQuery>()
            .ForMember(d => d.ChatId, m => m.Ignore());
        CreateMap<GetMessagesQueryResponse, GetMessagesResponse>();
        CreateMap<Domain.ReadOnly.Chats.MessageView, MessageView>();
    }

    private void GetChatsMap()
    {
        CreateMap<Domain.ReadOnly.Chats.ChatMessageView, ChatMessageView>();
        CreateMap<Domain.ReadOnly.Chats.ChatView, ChatView>();
        CreateMap<GetChatsRequest, GetChatsQuery>();
        CreateMap<GetChatsQueryResponse, GetChatsResponse>();
        CreateMap<Domain.ReadOnly.Chats.UserChatView, UserChatView>();
    }

    private void GetMatchesMap()
    {
        CreateMap<(Domain.ReadOnly.Chats.MatchView Match, bool IsSeen), MatchView>()
            .ForMember(d => d.IsSeen, m => m.MapFrom(s => s.IsSeen))
            .ForMember(d => d.Id, m => m.MapFrom(s => s.Match.Id))
            .ForMember(d => d.User, m => m.MapFrom(s => s.Match.User));
        CreateMap<GetMatchesQueryResponse, GetMatchesResponse>();
        CreateMap<GetMatchesRequest, GetMatchesQuery>();
    }
}