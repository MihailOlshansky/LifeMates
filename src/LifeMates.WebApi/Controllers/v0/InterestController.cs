using AutoMapper;
using LifeMates.Core.Commands.Interest;
using LifeMates.Core.Queries.Interest;
using LifeMates.WebApi.Controllers.v0.Models;
using LifeMates.WebApi.Controllers.v0.Models.Interest.Create;
using LifeMates.WebApi.Controllers.v0.Models.Interest.Get;
using LifeMates.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LifeMates.WebApi.Controllers.v0;

[ApiController]
[Route("interests")]
public class InterestController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public InterestController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(GetInterestsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetInterests(
        CancellationToken cancellationToken)
    {
        var command = new GetInterestsQuery();

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailed)
        {
            return this.Process(result);
        }
        
        var interests = _mapper.Map<GetInterestsResponse>(result.Value);

        return Ok(interests);
    }
    
    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateInterest(
        [FromBody] CreateInterestRequest interest,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateInterestCommand>(interest);
    
        var result = await _mediator.Send(command, cancellationToken);

        return this.Process(result);
    }
    
    [Authorize]
    [HttpDelete("{interestId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteInterest(
        [FromRoute] long interestId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteInterestCommand(interestId);
        
        var result = await _mediator.Send(command, cancellationToken);

        return this.Process(result);
    }
}

