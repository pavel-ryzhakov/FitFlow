using FitFlow.Application.Visits;
using Microsoft.AspNetCore.Mvc;

namespace FitFlow.Api.Controllers;

[ApiController]
[Route("api/visits")]
public class VisitsController : ControllerBase
{
    private readonly IVisitService _visitService;

    public VisitsController(IVisitService visitService)
    {
        _visitService = visitService;
    }

    [HttpGet]
    public async Task<ActionResult<List<VisitDto>>> GetAll(CancellationToken cancellationToken)
    {
        var visits = await _visitService.GetAllAsync(cancellationToken);

        return Ok(visits);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<VisitDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var visit = await _visitService.GetByIdAsync(id, cancellationToken);

        if (visit is null)
        {
            return NotFound();
        }

        return Ok(visit);
    }

    [HttpGet("client/{clientId:guid}")]
    public async Task<ActionResult<List<VisitDto>>> GetByClientId(
        Guid clientId,
        CancellationToken cancellationToken)
    {
        var visits = await _visitService.GetByClientIdAsync(clientId, cancellationToken);

        return Ok(visits);
    }

    [HttpPost]
    public async Task<ActionResult<VisitDto>> Register(
        VisitRegistrationRequest request,
        CancellationToken cancellationToken)
    {
        var visit = await _visitService.RegisterAsync(request, cancellationToken);

        if (visit is null)
        {
            return BadRequest("Client, membership or training session is invalid.");
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = visit.Id },
            visit);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Cancel(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _visitService.CancelAsync(id, cancellationToken);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}