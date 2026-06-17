using FitFlow.Api.Extensions;
using FitFlow.Application.Trainers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FitFlow.Api.Controllers;

[ApiController]
[Route("api/trainers")]
public class TrainersController : ControllerBase
{
    private readonly ITrainerService _trainerService;
    private readonly IValidator<TrainerCreationRequest> _trainerCreationValidator;
    private readonly IValidator<TrainerUpdateRequest> _trainerUpdateValidator;

    public TrainersController(
        ITrainerService trainerService,
        IValidator<TrainerCreationRequest> trainerCreationValidator,
        IValidator<TrainerUpdateRequest> trainerUpdateValidator)
    {
        _trainerService = trainerService;
        _trainerCreationValidator = trainerCreationValidator;
        _trainerUpdateValidator = trainerUpdateValidator;
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
        var result = await _trainerService.GetByIdAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<TrainerDto>> Create(
        TrainerCreationRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _trainerCreationValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToErrorDictionary()));
        }

        var result = await _trainerService.CreateAsync(request, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        var trainer = result.Value!;

        return CreatedAtAction(
            nameof(GetById),
            new { id = trainer.Id },
            trainer);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        TrainerUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _trainerUpdateValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToErrorDictionary()));
        }

        var result = await _trainerService.UpdateAsync(id, request, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _trainerService.DeleteAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return NoContent();
    }
}