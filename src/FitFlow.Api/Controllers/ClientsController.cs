using FitFlow.Application.Clients;
using Microsoft.AspNetCore.Mvc;

namespace FitFlow.Api.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
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
        var client = await _clientService.GetByIdAsync(id, cancellationToken);

        if (client is null)
        {
            return NotFound();
        }

        return Ok(client);
    }

    [HttpPost]
    public async Task<ActionResult<ClientDto>> Create(
        ClientCreationRequest request,
        CancellationToken cancellationToken)
    {
        var client = await _clientService.CreateAsync(request, cancellationToken);

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
        var result = await _clientService.UpdateAsync(id, request, cancellationToken);

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
        var result = await _clientService.DeleteAsync(id, cancellationToken);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}