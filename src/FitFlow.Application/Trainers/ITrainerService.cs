using FitFlow.Application.Common.Results;

namespace FitFlow.Application.Trainers;

public interface ITrainerService
{
    Task<List<TrainerDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Result<TrainerDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<TrainerDto>> CreateAsync(TrainerCreationRequest request, CancellationToken cancellationToken = default);

    Task<Result> UpdateAsync(Guid id, TrainerUpdateRequest request, CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}