using FitFlow.Application.Trainers;
using Microsoft.AspNetCore.Mvc;

namespace FitFlow.Api.Controllers;

[ApiController]
[Route("api/trainers")]
public class TrainersController : ControllerBase
{
    private readonly ITrainerService _trainerService;

    public TrainersController(ITrainerService trainerService)
    {
        _trainerService = trainerService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TrainerDto>>> GetAll(CancellationToken cancellationToken)
    {
        var trainers = await _trainerService.GetAllAsync(cancellationToken);

        return Ok(trainers);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TrainerDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var trainer = await _trainerService.GetByIdAsync(id, cancellationToken);

        if (trainer is null)
        {
            return NotFound();
        }

        return Ok(trainer);
    }

    [HttpPost]
    public async Task<ActionResult<TrainerDto>> Create(
        TrainerCreationRequest request,
        CancellationToken cancellationToken)
    {
        var trainer = await _trainerService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = trainer.Id },
            trainer);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateTrainerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _trainerService.UpdateAsync(id, request, cancellationToken);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _trainerService.DeleteAsync(id, cancellationToken);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}