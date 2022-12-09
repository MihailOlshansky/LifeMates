using FluentResults;
using LifeMates.Domain.ReadOnly.Interests;
using LifeMates.Storage.SharedKernel.ReadOnlyRepositories;
using MediatR;

namespace LifeMates.Core.Queries.Interest;

public record GetInterestsQuery() : IRequest<Result<GetInterestsQueryResponse>>;

public record GetInterestsQueryResponse(ICollection<InterestView> InterestViews);

public class GetInterestsQueryHandler : IRequestHandler<GetInterestsQuery, Result<GetInterestsQueryResponse>>
{
    private readonly IInterestReadOnlyRepository _interestReadOnlyRepository;

    public GetInterestsQueryHandler(
        IInterestReadOnlyRepository interestReadOnlyRepository)
    {
        _interestReadOnlyRepository = interestReadOnlyRepository;
    }

    public async Task<Result<GetInterestsQueryResponse>> Handle(
        GetInterestsQuery request,
        CancellationToken cancellationToken)
    {
        var interests = await _interestReadOnlyRepository.Get(cancellationToken);
        
        return Result.Ok(new GetInterestsQueryResponse(interests));
    }
}
