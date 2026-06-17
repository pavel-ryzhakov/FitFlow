namespace FitFlow.Application.Clients;

public interface IClientService
{
    Task<List<ClientDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ClientDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ClientDto> CreateAsync(ClientCreationRequest request, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(Guid id, ClientUpdateRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}