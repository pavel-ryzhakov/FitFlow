namespace FitFlow.Application.Trainers;

public interface ITrainerService
{
    Task<List<TrainerDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TrainerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<TrainerDto> CreateAsync(TrainerCreationRequest request, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(Guid id, UpdateTrainerRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}