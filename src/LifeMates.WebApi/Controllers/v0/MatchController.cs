using AutoMapper;
using LifeMates.Core.Queries.Chat;
using LifeMates.WebApi.Controllers.v0.Models;
using LifeMates.WebApi.Controllers.v0.Models.Chat.GetMetches;
using LifeMates.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LifeMates.WebApi.Controllers.v0;

[ApiController]
[Route("match")]
// Такое имя выбрано, чтобы охладить трахание со стороны Ульяны. Она выебала весь мой мозг за это сука слово.
public class MatchController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public MatchController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(GetMatchesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetChats(
        [FromQuery] GetMatchesRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<GetMatchesQuery>(request);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailed)
        {
            return this.Process(result);
        }

        var chatsView = _mapper.Map<GetMatchesResponse>(result.Value);

        return Ok(chatsView);
    }
}
