using FitFlow.Application.Common.Results;

namespace FitFlow.Application.TrainingSessions;

public interface ITrainingSessionService
{
    Task<List<TrainingSessionDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Result<TrainingSessionDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<List<TrainingSessionDto>>> GetBySectionIdAsync(Guid sectionId, CancellationToken cancellationToken = default);

    Task<Result<List<TrainingSessionDto>>> GetByTrainerIdAsync(Guid trainerId, CancellationToken cancellationToken = default);

    Task<Result<TrainingSessionDto>> CreateAsync(TrainingSessionCreationRequest request, CancellationToken cancellationToken = default);

    Task<Result> UpdateAsync(Guid id, TrainingSessionUpdateRequest request, CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}