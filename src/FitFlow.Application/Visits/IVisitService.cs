using FitFlow.Application.Common.Results;

namespace FitFlow.Application.Visits;

public interface IVisitService
{
    Task<List<VisitDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Result<VisitDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<List<VisitDto>>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);

    Task<Result<VisitDto>> RegisterAsync(VisitRegistrationRequest request, CancellationToken cancellationToken = default);

    Task<Result> CancelAsync(Guid id, CancellationToken cancellationToken = default);
}