using AutoMapper;
using FluentAssertions;
using LifeMates.Domain.Models.Interests;
using LifeMates.Domain.ReadOnly.Interests;
using LifeMates.Storage;
using LifeMates.Storage.Profiles;
using LifeMates.Storage.ReadOnlyRepositories;
using LifeMates.Storage.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Tests.LifeMates.Core;

namespace Tests.LifeMates.Storage.ReadOnlyRepositories;

public class InterestReadOnlyRepositoryTests
{
    private InterestRepository _interestRepository;
    private InterestReadOnlyRepository _interestReadOnlyRepository;
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        var dbContext = PlugGenerator.DbContext();
        _mapper = PlugGenerator.EntityToDomainMapper();
        
        _interestRepository = new InterestRepository(dbContext);
        _interestReadOnlyRepository = new InterestReadOnlyRepository(dbContext, _mapper);
    }
    
    [Test]
    public async Task Get_ReturnsEmptyCollection_WhenNoInterestExists()
    {
        var interests = await _interestReadOnlyRepository.Get(new CancellationToken());

        interests.Should().BeEmpty();
    }

    [Test]
    public async Task Get_ReturnsRightCollection()
    {
        var firstInterest = new Interest(1, "interest");
        var secondInterest = new Interest(2, "another interest");
        var cancellationToken = new CancellationToken();
        var interests = new List<Interest>() { firstInterest, secondInterest };

        await _interestRepository.Create(firstInterest, cancellationToken);
        await _interestRepository.Create(secondInterest, cancellationToken);
        var interestsView = interests.Select(_mapper.Map<InterestView>);
        var bdInterestsView = (await _interestReadOnlyRepository.Get(cancellationToken)).ToList();

        bdInterestsView.Should().BeEquivalentTo(interestsView);
    } 

}