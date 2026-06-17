using FitFlow.Api.Auth;
using FitFlow.Api.Extensions;
using FitFlow.Application.Sections;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitFlow.Api.Controllers;

[ApiController]
[Route("api/sections")]
[Authorize(Policy = AuthorizationPolicyNames.TrainerAccess)]
public class SectionsController : ControllerBase
{
    private readonly ISectionService _sectionService;
    private readonly IValidator<SectionCreationRequest> _sectionCreationValidator;
    private readonly IValidator<SectionUpdateRequest> _sectionUpdateValidator;

    public SectionsController(
        ISectionService sectionService,
        IValidator<SectionCreationRequest> sectionCreationValidator,
        IValidator<SectionUpdateRequest> sectionUpdateValidator)
    {
        _sectionService = sectionService;
        _sectionCreationValidator = sectionCreationValidator;
        _sectionUpdateValidator = sectionUpdateValidator;
    }

    [HttpGet]
    [Authorize(Policy = AuthorizationPolicyNames.ManagementAccess)]
    public async Task<ActionResult<List<SectionDto>>> GetAll(CancellationToken cancellationToken)
    {
        var sections = await _sectionService.GetAllAsync(cancellationToken);

        return Ok(sections);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicyNames.ManagementAccess)]
    public async Task<ActionResult<SectionDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sectionService.GetByIdAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicyNames.ManagementAccess)]
    public async Task<ActionResult<SectionDto>> Create(
        SectionCreationRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _sectionCreationValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToErrorDictionary()));
        }

        var result = await _sectionService.CreateAsync(request, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        var section = result.Value!;

        return CreatedAtAction(
            nameof(GetById),
            new { id = section.Id },
            section);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicyNames.ManagementAccess)]
    public async Task<IActionResult> Update(
        Guid id,
        SectionUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _sectionUpdateValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToErrorDictionary()));
        }

        var result = await _sectionService.UpdateAsync(id, request, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicyNames.ManagementAccess)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sectionService.DeleteAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return NoContent();
    }
}