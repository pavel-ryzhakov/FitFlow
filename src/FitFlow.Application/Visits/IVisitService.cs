namespace FitFlow.Application.Visits;

public interface IVisitService
{
    Task<List<VisitDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<VisitDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<VisitDto>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);

    Task<VisitDto?> RegisterAsync(VisitRegistrationRequest request, CancellationToken cancellationToken = default);

    Task<bool> CancelAsync(Guid id, CancellationToken cancellationToken = default);
}