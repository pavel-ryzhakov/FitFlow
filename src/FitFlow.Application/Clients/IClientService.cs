using FitFlow.Application.Common.Results;

namespace FitFlow.Application.Clients;

public interface IClientService
{
    Task<List<ClientDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Result<ClientDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<ClientDto>> CreateAsync(ClientCreationRequest request, CancellationToken cancellationToken = default);

    Task<Result> UpdateAsync(Guid id, ClientUpdateRequest request, CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}