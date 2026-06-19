using FitFlow.Api.Extensions;
using FitFlow.Application.Clients;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FitFlow.Api.Auth;

namespace FitFlow.Api.Controllers;

[ApiController]
[Route("api/clients")]
[Authorize(Policy = AuthorizationPolicyNames.ManagementAccess)]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly IValidator<ClientCreationRequest> _clientCreationValidator;
    private readonly IValidator<ClientUpdateRequest> _clientUpdateValidator;

    public ClientsController(
        IClientService clientService,
        IValidator<ClientCreationRequest> clientCreationValidator,
        IValidator<ClientUpdateRequest> clientUpdateValidator)
    {
        _clientService = clientService;
        _clientCreationValidator = clientCreationValidator;
        _clientUpdateValidator = clientUpdateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<List<ClientDto>>> GetAll(CancellationToken cancellationToken)
    {
        var clients = await _clientService.GetAllAsync(cancellationToken);

        return Ok(clients);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ClientDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _clientService.GetByIdAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<ClientDto>> Create(
        ClientCreationRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _clientCreationValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return ValidationProblem(
                new ValidationProblemDetails(validationResult.ToErrorDictionary())
            );
        }

        var result = await _clientService.CreateAsync(request, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        var client = result.Value!;

        return CreatedAtAction(
            nameof(GetById),
            new { id = client.Id },
            client);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        ClientUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _clientUpdateValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return ValidationProblem(
                new ValidationProblemDetails(validationResult.ToErrorDictionary())
            );
        }

        var result = await _clientService.UpdateAsync(id, request, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicyNames.AdminOnly)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _clientService.DeleteAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return NoContent();
    }
}