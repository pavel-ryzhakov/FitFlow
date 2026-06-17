using FitFlow.Api.Extensions;
using FitFlow.Application.Visits;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FitFlow.Api.Controllers;

[ApiController]
[Route("api/visits")]
public class VisitsController : ControllerBase
{
    private readonly IVisitService _visitService;
    private readonly IValidator<VisitRegistrationRequest> _visitRegistrationValidator;

    public VisitsController(
        IVisitService visitService,
        IValidator<VisitRegistrationRequest> visitRegistrationValidator)
    {
        _visitService = visitService;
        _visitRegistrationValidator = visitRegistrationValidator;
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
        var result = await _visitService.GetByIdAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("client/{clientId:guid}")]
    public async Task<ActionResult<List<VisitDto>>> GetByClientId(
        Guid clientId,
        CancellationToken cancellationToken)
    {
        var result = await _visitService.GetByClientIdAsync(clientId, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<VisitDto>> Register(
        VisitRegistrationRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _visitRegistrationValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToErrorDictionary()));
        }

        var result = await _visitService.RegisterAsync(request, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        var visit = result.Value!;

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

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return NoContent();
    }
}