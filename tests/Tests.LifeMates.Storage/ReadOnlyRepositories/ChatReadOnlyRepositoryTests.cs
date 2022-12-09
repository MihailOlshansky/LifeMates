using AutoMapper;
using FluentAssertions;
using LifeMates.Domain.Models.Chats;
using LifeMates.Domain.Models.Users;
using LifeMates.Domain.ReadOnly.Chats;
using LifeMates.Domain.ReadOnly.Users;
using LifeMates.Domain.Shared.Users;
using LifeMates.Storage.ReadOnlyRepositories;
using LifeMates.Storage.Repositories;
using LifeMates.Storage.SharedKernel.ReadOnlyRepositories;
using LifeMates.Storage.SharedKernel.Repositories;
using NUnit.Framework;
using Tests.LifeMates.Core;

namespace Tests.LifeMates.Storage.ReadOnlyRepositories;

public class ChatReadOnlyRepositoryTests
{
    private IChatReadOnlyRepository _chatReadOnlyRepository;
    private IUserRepository _userRepository ;
    private IMessageRepository _messageRepository;
    private IMapper _mapper;

    [SetUp]
    public async Task Setup()
    {
        var dbContext = PlugGenerator.DbContext();
        _mapper = PlugGenerator.EntityToDomainMapper();
        _userRepository = new UserRepository(dbContext);
        var chatRepository = new ChatRepository(dbContext);
        _chatReadOnlyRepository = new ChatReadOnlyRepository(_mapper, dbContext);
        var userLikesRepository = new UserLikesRepository(dbContext);
        _messageRepository = new MessageRepository(dbContext);

        await _userRepository.Create(new User(1, "user1", "description", UserGender.Man, UserStatus.Approved,
                DateTime.Now, new UserLocation(0, 0), new UserSettings(1, 1, UserGender.Woman),
                new List<UserImage>(), new List<UserContact>(), new List<UserInterest>()),
            CancellationToken.None);
        await _userRepository.Create(new User(2, "user2", "description", UserGender.Woman, UserStatus.Approved,
                DateTime.Now, new UserLocation(0, 0), new UserSettings(2, 2, UserGender.Man),
                new List<UserImage>(), new List<UserContact>(), new List<UserInterest>()),
            CancellationToken.None);

        userLikesRepository.Create(new UserLikes(1, 1, 2, DateTime.Now));
        userLikesRepository.Create(new UserLikes(2, 2, 1, DateTime.Now));

        var chatUserA = new ChatUser(1, true);
        var chatUserB = new ChatUser(2, true);
        
        
        var chat = new Chat(chatUserA, chatUserB);
        chat.AddMessage(1, "Hello, user2" );
        chat.AddMessage(2, "Hello, user1" );
        chatRepository.Create(chat);

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

    [Test]
    public async Task GetMatches_ReturnsRightMatchesCollection_OnValidParameters()
    {
        var matches = await _chatReadOnlyRepository.GetMatches(1, 0, 1, CancellationToken.None);
        var user = _mapper.Map<UserView>(await _userRepository.Get(2, CancellationToken.None));
        var expectedMatches = new List<MatchView>()
        {
            new MatchView()
            {
                Id = 1,
                User = new UserView()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Description = user.Description,
                    Birthday = user.Birthday,
                    Gender = user.Gender,
                    Status = user.Status,
                    Contacts = user.Contacts,
                    ImagesUrls = user.ImagesUrls,
                    Interests = user.Interests,
                }
            }
        };

        matches.Should().BeEquivalentTo(expectedMatches);
    }

    [Test]
    public async Task GetChats_ReturnsRightChatsCollection_OnValidParameters()
    {
        var chats = await _chatReadOnlyRepository.GetChats(1, 0, 2, CancellationToken.None);
        var user = _mapper.Map<UserView>(await _userRepository.Get(2, CancellationToken.None));
        var messages = await _messageRepository.Get(1, 2, 0, CancellationToken.None);
        var message = messages.First(m => m.UserId == user.Id);
        var userChatView = new UserChatView()
        {
            Id = user.Id,
            Name = user.Name,
            ImagesUrls = user.ImagesUrls
        };
        var expectedChats = new List<ChatView>()
        {
            new()
            {
                Id = 1,
                User = userChatView,
                Message = _mapper.Map<ChatMessageView>(message)
            }
        };

        chats.Should().BeEquivalentTo(expectedChats);
    }
    
}