using AutoMapper;
using LifeMates.Domain.ReadOnly.Interests;
using LifeMates.Storage.SharedKernel.ReadOnlyRepositories;
using Microsoft.EntityFrameworkCore;

namespace LifeMates.Storage.ReadOnlyRepositories;

public class InterestReadOnlyRepository : IInterestReadOnlyRepository
{
    private readonly LifematesDbContext _dbContext;
    private readonly IMapper _mapper;

    public InterestReadOnlyRepository(LifematesDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<ICollection<InterestView>> Get(CancellationToken cancellationToken)
    {
        var interests = await _dbContext.Interests.ToListAsync(cancellationToken);
        return interests.Select(_mapper.Map<InterestView>).ToList();
    }
}