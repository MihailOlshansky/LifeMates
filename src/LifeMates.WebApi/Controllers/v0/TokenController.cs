using AutoMapper;
using LifeMates.Core.Commands.Token;
using LifeMates.WebApi.Controllers.v0.Models;
using LifeMates.WebApi.Controllers.v0.Models.Token.Invoke;
using LifeMates.WebApi.Controllers.v0.Models.Token.Refresh;
using LifeMates.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LifeMates.WebApi.Controllers.v0;

[ApiController]
[Route("token")]
public class TokenController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public TokenController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RefreshTokenCommand>(request);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailed)
        {
            return this.Process(result);
        }

        var response = _mapper.Map<RefreshTokenResponse>(result.Value);
        
        return Ok(response);
    }

    [HttpPost("revoke")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Revoke(
        [FromBody] RevokeTokenRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RevokeTokenCommand>(request);

        var result = await _mediator.Send(command, cancellationToken);

        return this.Process(result);
    }
}