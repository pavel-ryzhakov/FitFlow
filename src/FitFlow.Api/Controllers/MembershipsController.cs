using FitFlow.Application.Memberships;
using Microsoft.AspNetCore.Mvc;

namespace FitFlow.Api.Controllers;

[ApiController]
[Route("api/memberships")]
public class MembershipsController : ControllerBase
{
    private readonly IMembershipService _membershipService;

    public MembershipsController(IMembershipService membershipService)
    {
        _membershipService = membershipService;
    }

    [HttpGet]
    public async Task<ActionResult<List<MembershipDto>>> GetAll(CancellationToken cancellationToken)
    {
        var memberships = await _membershipService.GetAllAsync(cancellationToken);

        return Ok(memberships);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MembershipDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var membership = await _membershipService.GetByIdAsync(id, cancellationToken);

        if (membership is null)
        {
            return NotFound();
        }

        return Ok(membership);
    }

    [HttpGet("client/{clientId:guid}")]
    public async Task<ActionResult<List<MembershipDto>>> GetByClientId(
        Guid clientId,
        CancellationToken cancellationToken)
    {
        var memberships = await _membershipService.GetByClientIdAsync(clientId, cancellationToken);

        return Ok(memberships);
    }

    [HttpPost]
    public async Task<ActionResult<MembershipDto>> Create(
        MembershipCreationRequest request,
        CancellationToken cancellationToken)
    {
        var membership = await _membershipService.CreateAsync(request, cancellationToken);

        if (membership is null)
        {
            return BadRequest("Client not found or membership data is invalid.");
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = membership.Id },
            membership);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        MembershipUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _membershipService.UpdateAsync(id, request, cancellationToken);

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
        var result = await _membershipService.DeleteAsync(id, cancellationToken);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}