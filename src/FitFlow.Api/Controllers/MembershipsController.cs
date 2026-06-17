using FitFlow.Api.Extensions;
using FitFlow.Application.Memberships;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FitFlow.Api.Controllers;

[ApiController]
[Route("api/memberships")]
public class MembershipsController : ControllerBase
{
    private readonly IMembershipService _membershipService;
    private readonly IValidator<MembershipCreationRequest> _membershipCreationValidator;
    private readonly IValidator<MembershipUpdateRequest> _membershipUpdateValidator;

    public MembershipsController(
        IMembershipService membershipService,
        IValidator<MembershipCreationRequest> membershipCreationValidator,
        IValidator<MembershipUpdateRequest> membershipUpdateValidator)
    {
        _membershipService = membershipService;
        _membershipCreationValidator = membershipCreationValidator;
        _membershipUpdateValidator = membershipUpdateValidator;
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
        var result = await _membershipService.GetByIdAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("client/{clientId:guid}")]
    public async Task<ActionResult<List<MembershipDto>>> GetByClientId(
        Guid clientId,
        CancellationToken cancellationToken)
    {
        var result = await _membershipService.GetByClientIdAsync(clientId, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<MembershipDto>> Create(
        MembershipCreationRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _membershipCreationValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToErrorDictionary()));
        }

        var result = await _membershipService.CreateAsync(request, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        var membership = result.Value!;

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
        var validationResult = await _membershipUpdateValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToErrorDictionary()));
        }

        var result = await _membershipService.UpdateAsync(id, request, cancellationToken);

        if (result.IsFailure)
        {
            if (result.Error == MembershipErrors.NotFound)
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
        var result = await _membershipService.DeleteAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return NoContent();
    }
}