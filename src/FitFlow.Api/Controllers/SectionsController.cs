using FitFlow.Application.Sections;
using Microsoft.AspNetCore.Mvc;

namespace FitFlow.Api.Controllers;

[ApiController]
[Route("api/sections")]
public class SectionsController : ControllerBase
{
    private readonly ISectionService _sectionService;

    public SectionsController(ISectionService sectionService)
    {
        _sectionService = sectionService;
    }

    [HttpGet]
    public async Task<ActionResult<List<SectionDto>>> GetAll(CancellationToken cancellationToken)
    {
        var sections = await _sectionService.GetAllAsync(cancellationToken);

        return Ok(sections);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SectionDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var section = await _sectionService.GetByIdAsync(id, cancellationToken);

        if (section is null)
        {
            return NotFound();
        }

        return Ok(section);
    }

    [HttpPost]
    public async Task<ActionResult<SectionDto>> Create(
        SectionCreationRequest request,
        CancellationToken cancellationToken)
    {
        var section = await _sectionService.CreateAsync(request, cancellationToken);

        if (section is null)
        {
            return BadRequest("Trainer not found.");
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = section.Id },
            section);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        SectionUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sectionService.UpdateAsync(id, request, cancellationToken);

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
        var result = await _sectionService.DeleteAsync(id, cancellationToken);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}