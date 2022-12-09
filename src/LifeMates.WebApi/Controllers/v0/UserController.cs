using AutoMapper;
using LifeMates.Core.Commands.Match;
using LifeMates.Core.Commands.User;
using LifeMates.Core.Queries.Users;
using LifeMates.WebApi.Controllers.v0.Models;
using LifeMates.WebApi.Controllers.v0.Models.User;
using LifeMates.WebApi.Controllers.v0.Models.User.Details;
using LifeMates.WebApi.Controllers.v0.Models.User.Likes;
using LifeMates.WebApi.Controllers.v0.Models.User.Search;
using LifeMates.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserStatus = LifeMates.WebApi.Controllers.v0.Models.User.Details.UserStatus;

namespace LifeMates.WebApi.Controllers.v0;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(SearchResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Search(
        [FromQuery] SearchRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<SearchUserQuery>(request);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailed)
        {
            return this.Process(result);
        }

        var searchMates = _mapper.Map<SearchResponse>(result.Value);
        
        return Ok(searchMates);
    }
    
    [Authorize]
    [HttpGet("{userId:long}")]
    [ProducesResponseType(typeof(UserView), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Get(
        [FromRoute] long userId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserQuery(userId), cancellationToken);

        if (result.IsFailed)
        {
            return this.Process(result);
        }

        var userView = _mapper.Map<UserView>(result.Value);

        return Ok(userView);
    }

    [Authorize]
    [HttpPost("{userId:long}/like")]
    [ProducesResponseType(typeof(LikeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Like(
        [FromRoute] long userId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new LikeCommand(userId), cancellationToken);

        if (result.IsFailed)
        {
            return this.Process(result);
        }

        var userView = _mapper.Map<LikeResponse>(result.Value);

        return Ok(userView);
    }


    [Authorize]
    [HttpPost("{userId:long}/dislike")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Dislike(
        [FromRoute] long userId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DislikeCommand(userId), cancellationToken);

        return this.Process(result);
    }

    [Authorize]
    [HttpPost("{userId:long}/status/{newStatus}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeStatus(
        [FromRoute] long userId,
        [FromRoute] UserStatus newStatus,
        CancellationToken cancellationToken)
    {
        var status = _mapper.Map<Domain.Shared.Users.UserStatus>(newStatus);
        var command = new EditUserStatusCommand(userId, status);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsFailed
            ? this.Process(result)
            : Ok();
    }

}