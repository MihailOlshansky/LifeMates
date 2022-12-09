using AutoMapper;
using LifeMates.Core.Commands.Chats;
using LifeMates.Core.Queries.Chat;
using LifeMates.WebApi.Controllers.v0.Models;
using LifeMates.WebApi.Controllers.v0.Models.Chat.GetChats;
using LifeMates.WebApi.Controllers.v0.Models.Chat.GetMessages;
using LifeMates.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LifeMates.WebApi.Controllers.v0;

[Authorize]
[ApiController]
[Route("chats")]
public class ChatsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ChatsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetChatsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetChats(
        [FromQuery] GetChatsRequest request,
        CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetChatsQuery>(request);

        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailed)
        {
            return this.Process(result);
        }

        var chatsView = _mapper.Map<GetChatsResponse>(result.Value);

        return Ok(chatsView);
    }

    [HttpPost("{chatId:long}/message")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateMessage(
        [FromRoute] long chatId,
        [FromBody] string content,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new CreateMessageCommand(chatId, content), cancellationToken);

        return this.Process(result);
    }
    
    [HttpGet("{chatId:long}/message")]
    [ProducesResponseType(typeof(GetMessagesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMessages(
        [FromRoute] long chatId,
        [FromQuery] GetMessagesRequest request,
        CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetMessagesQuery>(request);
        query.ChatId = chatId;

        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailed)
        {
            return this.Process(result);
        }

        var chatsView = _mapper.Map<GetMessagesResponse>(result.Value);

        return Ok(chatsView);
    }
}
