using AutoMapper;
using LifeMates.Core.Commands.User;
using LifeMates.Core.Queries.Users;
using LifeMates.WebApi.Controllers.v0.Models;
using LifeMates.WebApi.Controllers.v0.Models.User;
using LifeMates.WebApi.Controllers.v0.Models.User.Edit;
using LifeMates.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LifeMates.WebApi.Controllers.v0;

[ApiController]
[Route("me")]
public class MeController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public MeController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(ProfileView), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Get(
        CancellationToken cancellationToken)
    {
        var command = new GetMeQuery();

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailed)
        {
            return this.Process(result);
        }

        var profileView = _mapper.Map<ProfileView>(result.Value);

        return Ok(profileView);
    }
    
    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Edit(
        [FromBody] EditUserRequest profile,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<EditUserCommand>(profile);

        var result = await _mediator.Send(command, cancellationToken);

        return this.Process(result);
    }
    
    [Authorize]
    [HttpPost("location")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> EditLocation(
        [FromBody] EditUserLocationRequest? profile,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<EditUserLocationCommand>(profile);

        var result = await _mediator.Send(command, cancellationToken);

        return this.Process(result);
    }

    [Authorize]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(
        CancellationToken cancellationToken)
    {
        var command = new DeleteMeCommand();

        var result = await _mediator.Send(command, cancellationToken);

        return this.Process(result);
    }
}