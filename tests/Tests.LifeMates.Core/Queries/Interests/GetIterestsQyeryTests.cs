using AutoMapper;
using FluentAssertions;
using LifeMates.Core.Queries.Interest;
using LifeMates.Domain.Models.Interests;
using LifeMates.Domain.ReadOnly.Interests;
using LifeMates.Storage;
using LifeMates.Storage.ReadOnlyRepositories;
using LifeMates.Storage.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Tests.LifeMates.Core.Queries.Interests;

public class GetInterestsQueryTests
{
    private LifematesDbContext _dbContext;
    private IMapper _mapper;
    private GetInterestsQueryHandler _handler;
    
    [SetUp]
    public void Setup()
    {
        _dbContext = PlugGenerator.DbContext();
        _mapper = PlugGenerator.EntityToDomainMapper();
        var interestsRepository = new InterestReadOnlyRepository(_dbContext, _mapper);

        _handler = new GetInterestsQueryHandler(interestsRepository);

    }

    [Test]
    public void Handle_ReturnsEmptyCollection_WhenNoInterestExists()
    {
        var cancellationToken = new CancellationToken();
        var result = _handler.Handle(
            new GetInterestsQuery(),
            cancellationToken);

        var size = result.WaitAsync(cancellationToken).Result.Value.InterestViews.Count;
        size.Should().Be(0);
    }

    [Test]
    public void Handle_ReturnsInterestsCollection_WhenAnyInterestExists()
    {
        var cancellationToken = new CancellationToken();
        var interestsRepository = new InterestRepository(_dbContext);
        var interest = new Interest(1, "aboba");
        
        interestsRepository.Create(interest, cancellationToken).WaitAsync(cancellationToken);
        var result = _handler.Handle(
            new GetInterestsQuery(),
            cancellationToken);

        var interests = result.WaitAsync(cancellationToken).Result.Value.InterestViews; 
        interests.Should().BeEquivalentTo(new List<InterestView>(){_mapper.Map<InterestView>(interest)});
    }
}