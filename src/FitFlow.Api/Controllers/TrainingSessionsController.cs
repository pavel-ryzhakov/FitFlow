using FitFlow.Api.Extensions;
using FitFlow.Application.TrainingSessions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FitFlow.Api.Controllers;

[ApiController]
[Route("api/training-sessions")]
public class TrainingSessionsController : ControllerBase
{
    private readonly ITrainingSessionService _trainingSessionService;
    private readonly IValidator<TrainingSessionCreationRequest> _trainingSessionCreationValidator;
    private readonly IValidator<TrainingSessionUpdateRequest> _trainingSessionUpdateValidator;

    public TrainingSessionsController(
        ITrainingSessionService trainingSessionService,
        IValidator<TrainingSessionCreationRequest> trainingSessionCreationValidator,
        IValidator<TrainingSessionUpdateRequest> trainingSessionUpdateValidator)
    {
        _trainingSessionService = trainingSessionService;
        _trainingSessionCreationValidator = trainingSessionCreationValidator;
        _trainingSessionUpdateValidator = trainingSessionUpdateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<List<TrainingSessionDto>>> GetAll(CancellationToken cancellationToken)
    {
        var trainingSessions = await _trainingSessionService.GetAllAsync(cancellationToken);

        return Ok(trainingSessions);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TrainingSessionDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _trainingSessionService.GetByIdAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("section/{sectionId:guid}")]
    public async Task<ActionResult<List<TrainingSessionDto>>> GetBySectionId(
        Guid sectionId,
        CancellationToken cancellationToken)
    {
        var result = await _trainingSessionService.GetBySectionIdAsync(sectionId, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("trainer/{trainerId:guid}")]
    public async Task<ActionResult<List<TrainingSessionDto>>> GetByTrainerId(
        Guid trainerId,
        CancellationToken cancellationToken)
    {
        var result = await _trainingSessionService.GetByTrainerIdAsync(trainerId, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<TrainingSessionDto>> Create(
        TrainingSessionCreationRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _trainingSessionCreationValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToErrorDictionary()));
        }

        var result = await _trainingSessionService.CreateAsync(request, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        var trainingSession = result.Value!;

        return CreatedAtAction(
            nameof(GetById),
            new { id = trainingSession.Id },
            trainingSession);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        TrainingSessionUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _trainingSessionUpdateValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToErrorDictionary()));
        }

        var result = await _trainingSessionService.UpdateAsync(id, request, cancellationToken);

        if (result.IsFailure)
        {
            if (result.Error == TrainingSessionErrors.NotFound)
            {
                return NotFound(result.Error);
            }

            return BadRequest(result.Error);
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _trainingSessionService.DeleteAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            if (result.Error == TrainingSessionErrors.NotFound)
            {
                return NotFound(result.Error);
            }

            return BadRequest(result.Error);
        }

        return NoContent();
    }
}