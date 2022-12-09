using AutoMapper;
using FluentAssertions;
using LifeMates.Domain.Constant;
using LifeMates.Domain.Models.Users;
using LifeMates.Domain.ReadOnly.Users;
using LifeMates.Domain.Shared.Users;
using LifeMates.Storage;
using LifeMates.Storage.ReadOnlyRepositories;
using LifeMates.Storage.Repositories;
using LifeMates.Storage.SharedKernel.ReadOnlyRepositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Tests.LifeMates.Core;

namespace Tests.LifeMates.Storage.ReadOnlyRepositories;

public class UserReadOnlyRepositoryTests
{
    private const int AmountMultiplier = 3;
    private const double CoordStep = 0.01;
    
    private UserRepository _userRepository;
    private UserReadOnlyRepository _userReadOnlyRepository;
    private IMapper _mapper;
    private User _defaultUser;
    private LifematesDbContext _dbContext;

    [SetUp]
    public void SetUp()
    {
        _dbContext = PlugGenerator.DbContext();
        _mapper = PlugGenerator.EntityToDomainMapper();

        _userRepository = new UserRepository(_dbContext);
        _userReadOnlyRepository = new UserReadOnlyRepository(_mapper, _dbContext);
        _defaultUser = new User(1, "user", "description", UserGender.Man, UserStatus.Approved, null,null,
            new UserSettings(1, 1, UserGender.Woman), new List<UserImage>(), new List<UserContact>(), new List<UserInterest>());
    }

    [Test]
    public async Task GetMe_ReturnsRightUser_WhenUserExists()
    {
        var me = _mapper.Map<MeView>(_defaultUser);

        await _userRepository.Create(_defaultUser, CancellationToken.None);
        var bdUser = await _userReadOnlyRepository.GetMe(_defaultUser.Id, CancellationToken.None);

        bdUser.Should().BeEquivalentTo(me);
    }

    [Test]
    public async Task GetMe_ReturnsNull_WhenUserDoesNotExist()
    {
        var cancellationToken = new CancellationToken();

        var bdUser = await _userReadOnlyRepository.GetMe(1, cancellationToken);

        bdUser.Should().BeNull();
    }

    [Test]
    public async Task Get_ReturnsRightUser_WhenUserExists()
    {
        var cancellationToken = new CancellationToken();
        var userView = _mapper.Map<UserView>(_defaultUser);

        await _userRepository.Create(_defaultUser, cancellationToken);
        var bdUser = await _userReadOnlyRepository.Get(_defaultUser.Id, cancellationToken);

        bdUser.Should().BeEquivalentTo(userView);
    }

    [Test]
    public async Task Get_ReturnsNull_WhenUserDoesNotExist()
    {
        var cancellationToken = new CancellationToken();

        var bdUser = await _userReadOnlyRepository.Get(1, cancellationToken);

        bdUser.Should().BeNull();
    }

    [TestCaseSource(typeof(SearchClass), nameof(SearchClass.ValidSearchFiltersLocationTesting))]
    public async Task Search_ReturnsCollectionOfUsersInRadius(SearchFilter searchFilter)
    { 
        LocalDbContext();
        var searchedUsers = await _userReadOnlyRepository.Search(searchFilter, CancellationToken.None);

        var isRightCollection = searchFilter.Latitude == null ||
                                searchFilter.Longitude == null ||
                                searchedUsers.All(u =>
                                    u.Location == null ||
                                    Math.Abs(u.Location.Latitude - searchFilter.Latitude.Value) < searchFilter.Radius && 
                                    Math.Abs(u.Location.Longitude - searchFilter.Longitude.Value) < searchFilter.Radius);

        isRightCollection.Should().BeTrue();
    }
    
    [Test]
    public async Task Search_ReturnsCollectionOfSizeNotExceededLimit()
    { 
        LocalDbContext();
        var searchFilter = new SearchFilter()
        {
            SearchForUserId = 16,
            Limit = 10,
            Offset = 0,
            Radius = 10,
            Latitude = 6 * CoordStep,
            Longitude = 6 * CoordStep,
            SearchingGender = UserGender.Man
        };

        var searchedUsers = await _userReadOnlyRepository.Search(searchFilter, CancellationToken.None);

        var isRightCollection = searchedUsers.Count <= searchFilter.Limit;

        isRightCollection.Should().BeTrue();
    }

    [TestCaseSource(typeof(SearchClass), nameof(SearchClass.ValidSearchFiltersSearchingGenderTesting))]
    public async Task Search_ReturnsCollectionOfSearchedGenderUsers(SearchFilter searchFilter)
    { 
        LocalDbContext();

        var searchedUsers = await _userReadOnlyRepository.Search(searchFilter, CancellationToken.None);

        var isRightCollection = searchedUsers.All(u => u.Gender == searchFilter.SearchingGender);

        isRightCollection.Should().BeTrue();
    }
    
    [Test]
    public async Task Search_ReturnsCollectionOfNotLikedUsers()
    { 
        LocalDbContext();
        var searchFilter = new SearchFilter()
        {
            SearchForUserId = 16,
            Limit = 1000,
            Offset = 0,
            Radius = 10,
            Latitude = 6 * CoordStep,
            Longitude = 6 * CoordStep,
            SearchingGender = UserGender.Man
        };
        var searchedUsers = await _userReadOnlyRepository.Search(searchFilter, CancellationToken.None);

        var userLikesRepository = new UserLikesRepository(_dbContext);
        var isRightCollection = searchedUsers.All(u => !userLikesRepository
            .Exist(searchFilter.SearchForUserId, u.Id, CancellationToken.None)
            .WaitAsync(CancellationToken.None)
            .Result);

        isRightCollection.Should().BeTrue();
    }
    
    [Test]
    public async Task Search_ReturnsCollectionOfNotDislikedUsers()
    { 
        LocalDbContext();
        var searchFilter = new SearchFilter()
        {
            SearchForUserId = 16,
            Limit = 1000,
            Offset = 0,
            Radius = 10,
            Latitude = 6 * CoordStep,
            Longitude = 6 * CoordStep,
            SearchingGender = UserGender.Man
        };

        var searchedUsers = await _userReadOnlyRepository.Search(searchFilter, CancellationToken.None);

        var userDislikesRepository = new UserDislikeRepository(_dbContext);
        var isRightCollection = searchedUsers.All(u => !userDislikesRepository
            .Exist(searchFilter.SearchForUserId, u.Id, CancellationToken.None)
            .WaitAsync(CancellationToken.None)
            .Result);

        isRightCollection.Should().BeTrue();
    }

    private void LocalDbContext()
    {
        _dbContext = new LifematesDbContext(new DbContextOptionsBuilder<LifematesDbContext>()
            .UseSqlServer("Server=localhost\\sqlexpress;Database=LifeMates;Trusted_Connection=True;")
            .Options);
        _userRepository = new UserRepository(_dbContext);
        _userReadOnlyRepository = new UserReadOnlyRepository(_mapper, _dbContext);
    }
    
    private class SearchClass
    {
        public static IEnumerable<TestCaseData> ValidSearchFiltersLocationTesting()
        {
            yield return new TestCaseData(new SearchFilter()
            {
                SearchForUserId = 16,
                Limit = 10,
                Offset = 0,
                Radius = Constants.Users.Match.Radius,
                Latitude = 6 * CoordStep,
                Longitude = 6 * CoordStep,
                SearchingGender = UserGender.Man
            }).SetName("Location is not null");
            yield return new TestCaseData(new SearchFilter()
            {
                SearchForUserId = 16,
                Limit = 10,
                Offset = 0,
                Radius = Constants.Users.Match.Radius,
                Latitude = null,
                Longitude = null,
                SearchingGender = UserGender.Man
            }).SetName("Location is null");
            yield return new TestCaseData(new SearchFilter()
            {
                SearchForUserId = 16,
                Limit = 10,
                Offset = 0,
                Radius = Constants.Users.Match.Radius,
                Latitude = 6 * CoordStep,
                Longitude = null,
                SearchingGender = UserGender.Man
            }).SetName("Longitude is null");
            yield return new TestCaseData(new SearchFilter()
            {
                SearchForUserId = 16,
                Limit = 10,
                Offset = 0,
                Radius = Constants.Users.Match.Radius,
                Latitude = null,
                Longitude = 6 * CoordStep,
                SearchingGender = UserGender.Man
            }).SetName("Latitude is null");
        }
        
        public static IEnumerable<TestCaseData> ValidSearchFiltersSearchingGenderTesting()
        {
            foreach (var userGender in Enum.GetValues<UserGender>())
            {
                yield return new TestCaseData(new SearchFilter()
                {
                    SearchForUserId = 16,
                    Limit = 10,
                    Offset = 0,
                    Radius = Constants.Users.Match.Radius,
                    Latitude = 6 * CoordStep,
                    Longitude = 6 * CoordStep,
                    SearchingGender = userGender
                }).SetName("Searching gender is " + userGender.ToString());
            }
        }
    }
}