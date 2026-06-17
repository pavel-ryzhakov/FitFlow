using FitFlow.Application.TrainingSessions;
using Microsoft.AspNetCore.Mvc;

namespace FitFlow.Api.Controllers;

[ApiController]
[Route("api/training-sessions")]
public class TrainingSessionsController : ControllerBase
{
    private readonly ITrainingSessionService _trainingSessionService;

    public TrainingSessionsController(ITrainingSessionService trainingSessionService)
    {
        _trainingSessionService = trainingSessionService;
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
        var trainingSession = await _trainingSessionService.GetByIdAsync(id, cancellationToken);

        if (trainingSession is null)
        {
            return NotFound();
        }

        return Ok(trainingSession);
    }

    [HttpGet("section/{sectionId:guid}")]
    public async Task<ActionResult<List<TrainingSessionDto>>> GetBySectionId(
        Guid sectionId,
        CancellationToken cancellationToken)
    {
        var trainingSessions = await _trainingSessionService.GetBySectionIdAsync(sectionId, cancellationToken);

        return Ok(trainingSessions);
    }

    [HttpGet("trainer/{trainerId:guid}")]
    public async Task<ActionResult<List<TrainingSessionDto>>> GetByTrainerId(
        Guid trainerId,
        CancellationToken cancellationToken)
    {
        var trainingSessions = await _trainingSessionService.GetByTrainerIdAsync(trainerId, cancellationToken);

        return Ok(trainingSessions);
    }

    [HttpPost]
    public async Task<ActionResult<TrainingSessionDto>> Create(
        TrainingSessionCreationRequest request,
        CancellationToken cancellationToken)
    {
        var trainingSession = await _trainingSessionService.CreateAsync(request, cancellationToken);

        if (trainingSession is null)
        {
            return BadRequest("Training session data is invalid.");
        }

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
        var result = await _trainingSessionService.UpdateAsync(id, request, cancellationToken);

        if (!result)
        {
            return BadRequest("Training session was not found or data is invalid.");
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _trainingSessionService.DeleteAsync(id, cancellationToken);

        if (!result)
        {
            return BadRequest("Training session was not found or already has visits.");
        }

        return NoContent();
    }
}