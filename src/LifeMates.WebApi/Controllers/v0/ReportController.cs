using AutoMapper;
using LifeMates.Core.Commands.Reports;
using LifeMates.WebApi.Controllers.v0.Models;
using LifeMates.WebApi.Controllers.v0.Models.Reports;
using LifeMates.WebApi.Controllers.v0.Models.Reports.Create;
using LifeMates.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LifeMates.WebApi.Controllers.v0;

[Authorize]
[ApiController]
[Route("reports")]
public class ReportController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ReportController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateReportResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiBadResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] CreateReportRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateReportCommand>(request);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailed)
        {
            return this.Process(result);
        }

        var chatsView = new CreateReportResponse
        {
            Report = _mapper.Map<ReportView>(result.Value)
        };
        
        return Ok(chatsView);
    }
}