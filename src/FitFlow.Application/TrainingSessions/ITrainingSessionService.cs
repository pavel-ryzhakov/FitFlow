namespace FitFlow.Application.TrainingSessions;

public interface ITrainingSessionService
{
    Task<List<TrainingSessionDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TrainingSessionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<TrainingSessionDto>> GetBySectionIdAsync(Guid sectionId, CancellationToken cancellationToken = default);

    Task<List<TrainingSessionDto>> GetByTrainerIdAsync(Guid trainerId, CancellationToken cancellationToken = default);

    Task<TrainingSessionDto?> CreateAsync(TrainingSessionCreationRequest request, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(Guid id, TrainingSessionUpdateRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}