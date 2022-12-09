using FluentAssertions;
using LifeMates.Domain.Models.Interests;
using LifeMates.Storage;
using LifeMates.Storage.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Tests.LifeMates.Core;

namespace Tests.LifeMates.Storage.Repositories;

public class InterestRepositoryTests
{
    private InterestRepository _interestRepository;

    [SetUp]
    public void SetUp()
    {
        var dbContext = PlugGenerator.DbContext();
        
        _interestRepository = new InterestRepository(dbContext);
    }

    [Test]
    public void Create_CompletesSuccessfully_OnValidParameters()
    {
        var interest = new Interest("interest");
        var cancellationToken = new CancellationToken();

        var result = _interestRepository.Create(interest, cancellationToken);
        var isCreated = result.WaitAsync(cancellationToken).IsCompletedSuccessfully;

        isCreated.Should().BeTrue();
    }

    [Test]
    public void Create_Fails_WhenInterestAlreadyExists()
    {
        var interest = new Interest("interest");
        var cancellationToken = new CancellationToken();

        _interestRepository.Create(interest, cancellationToken).WaitAsync(cancellationToken);
        var result = _interestRepository.Create(interest, cancellationToken);
        var isCreated = result.WaitAsync(cancellationToken).IsCompletedSuccessfully;

        isCreated.Should().BeFalse();
    }

    [Test]
    public async Task GetById_ReturnsRightUser_WhenInterestExists()
    {
        var interest = new Interest(1, "interest");
        var cancellationToken = new CancellationToken();

        await _interestRepository.Create(interest, cancellationToken);
        var bdInterest = await _interestRepository.GetById(1, cancellationToken);

        bdInterest.Should().BeEquivalentTo(interest);
    }

    [Test]
    public async Task GetById_ReturnsNull_WhenInterestDoesNotExist()
    {
        var bdInterest = await _interestRepository.GetById(1, new CancellationToken());

        bdInterest.Should().BeNull();
    }

    [Test]
    public async Task Remove_CompletesSuccessfully_WhenInterestExists()
    {
        var interest = new Interest(1, "interest");
        var cancellationToken = new CancellationToken();

        await _interestRepository.Create(interest, cancellationToken);
        var result= _interestRepository.Remove(interest, cancellationToken);
        var isDeleted = result.WaitAsync(cancellationToken).IsCompletedSuccessfully;
        
        isDeleted.Should().BeTrue();
    }

    [Test]
    public async Task Exists_ByValue_ReturnsTrue_WhenInterestExists()
    {
        var interest = new Interest("interest");
        var cancellationToken = new CancellationToken();

        await _interestRepository.Create(interest, cancellationToken);
        var isInterestExists = await _interestRepository.Exists(interest.Value, cancellationToken);

        isInterestExists.Should().BeTrue();
    }
    
    [Test]
    public async Task Exists_ByValue_ReturnsFalse_WhenInterestDoesNotExist()
    {
        var isInterestExists = await _interestRepository.Exists("interest", new CancellationToken());

        isInterestExists.Should().BeFalse();
    }
    
    [Test]
    public async Task Exists_ById_ReturnsTrue_WhenInterestExists()
    {
        var interest = new Interest(1, "interest");
        var cancellationToken = new CancellationToken();

        await _interestRepository.Create(interest, cancellationToken);
        var isInterestExists = await _interestRepository.Exists(interest.Id, cancellationToken);

        isInterestExists.Should().BeTrue();
    }
    
    [Test]
    public async Task Exists_ById_ReturnsFalse_WhenInterestDoesNotExist()
    {
        var isInterestExists = await _interestRepository.Exists(1, new CancellationToken());

        isInterestExists.Should().BeFalse();
    }
    
    [Test]
    public async Task Exists_ByIds_ReturnsTrue_WhenAllInterestsExist()
    {
        var firstInterest = new Interest(1, "interest");
        var secondInterest = new Interest(2, "another interest");
        var cancellationToken = new CancellationToken();
        var ids = new List<long>() { firstInterest.Id, secondInterest.Id };

        await _interestRepository.Create(firstInterest, cancellationToken);
        await _interestRepository.Create(secondInterest, cancellationToken);
        var isInterestExists = await _interestRepository.Exists(ids, cancellationToken);

        isInterestExists.Should().BeTrue();
    }
    
    [Test]
    public async Task Exists_ByIds_ReturnsFalse_WhenAllDoesNotExist()
    {
        var ids = new List<long>() { 1, 2, 3 };
        
        var isInterestExists = await _interestRepository.Exists(ids, new CancellationToken());

        isInterestExists.Should().BeFalse();
    }
    
    [Test]
    public async Task Exists_ByIds_ReturnsFalse_WhenSomeDoesNotExist()
    {
        var interest = new Interest(1, "interest");
        var cancellationToken = new CancellationToken();
        var ids = new List<long>() { interest.Id, 2 };

        await _interestRepository.Create(interest, cancellationToken);
        var isInterestExists = await _interestRepository.Exists(ids, cancellationToken);

        isInterestExists.Should().BeFalse();
    }
    
    [Test]
    public void Remove_Fails_WhenInterestDoesNotExist()
    {
        var interest = new Interest(1, "interest");
        var cancellationToken = new CancellationToken();

        var result= _interestRepository.Remove(interest, cancellationToken);
        var isDeleted = result.WaitAsync(cancellationToken).IsCompletedSuccessfully;

        isDeleted.Should().BeFalse();
    }
}