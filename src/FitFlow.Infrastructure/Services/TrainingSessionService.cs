using FitFlow.Application.Common.Results;
using FitFlow.Application.TrainingSessions;
using FitFlow.Domain.Entities;
using FitFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitFlow.Infrastructure.Services;

public class TrainingSessionService : ITrainingSessionService
{
    private readonly FitFlowDbContext _dbContext;

    public TrainingSessionService(FitFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TrainingSessionDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.TrainingSessions
            .AsNoTracking()
            .Include(x => x.Section)
            .Include(x => x.Trainer)
            .Include(x => x.Visits)
            .OrderBy(x => x.StartTime)
            .Select(x => new TrainingSessionDto
            {
                Id = x.Id,
                SectionId = x.SectionId,
                SectionName = x.Section == null ? string.Empty : x.Section.Name,
                TrainerId = x.TrainerId,
                TrainerFullName = x.Trainer == null
                    ? string.Empty
                    : x.Trainer.FirstName + " " + x.Trainer.LastName,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                MaxParticipants = x.MaxParticipants,
                VisitsCount = x.Visits.Count
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<TrainingSessionDto>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var trainingSession = await _dbContext.TrainingSessions
            .AsNoTracking()
            .Include(x => x.Section)
            .Include(x => x.Trainer)
            .Include(x => x.Visits)
            .Where(x => x.Id == id)
            .Select(x => new TrainingSessionDto
            {
                Id = x.Id,
                SectionId = x.SectionId,
                SectionName = x.Section == null ? string.Empty : x.Section.Name,
                TrainerId = x.TrainerId,
                TrainerFullName = x.Trainer == null
                    ? string.Empty
                    : x.Trainer.FirstName + " " + x.Trainer.LastName,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                MaxParticipants = x.MaxParticipants,
                VisitsCount = x.Visits.Count
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (trainingSession is null)
        {
            return Result<TrainingSessionDto>.Failure(TrainingSessionErrors.NotFound);
        }

        return Result<TrainingSessionDto>.Success(trainingSession);
    }

    public async Task<Result<List<TrainingSessionDto>>> GetBySectionIdAsync(
        Guid sectionId,
        CancellationToken cancellationToken = default)
    {
        var sectionExists = await _dbContext.Sections
            .AnyAsync(x => x.Id == sectionId, cancellationToken);

        if (!sectionExists)
        {
            return Result<List<TrainingSessionDto>>.Failure(TrainingSessionErrors.SectionNotFound);
        }

        var trainingSessions = await _dbContext.TrainingSessions
            .AsNoTracking()
            .Include(x => x.Section)
            .Include(x => x.Trainer)
            .Include(x => x.Visits)
            .Where(x => x.SectionId == sectionId)
            .OrderBy(x => x.StartTime)
            .Select(x => new TrainingSessionDto
            {
                Id = x.Id,
                SectionId = x.SectionId,
                SectionName = x.Section == null ? string.Empty : x.Section.Name,
                TrainerId = x.TrainerId,
                TrainerFullName = x.Trainer == null
                    ? string.Empty
                    : x.Trainer.FirstName + " " + x.Trainer.LastName,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                MaxParticipants = x.MaxParticipants,
                VisitsCount = x.Visits.Count
            })
            .ToListAsync(cancellationToken);

        return Result<List<TrainingSessionDto>>.Success(trainingSessions);
    }

    public async Task<Result<List<TrainingSessionDto>>> GetByTrainerIdAsync(
        Guid trainerId,
        CancellationToken cancellationToken = default)
    {
        var trainerExists = await _dbContext.Trainers
            .AnyAsync(x => x.Id == trainerId, cancellationToken);

        if (!trainerExists)
        {
            return Result<List<TrainingSessionDto>>.Failure(TrainingSessionErrors.TrainerNotFound);
        }

        var trainingSessions = await _dbContext.TrainingSessions
            .AsNoTracking()
            .Include(x => x.Section)
            .Include(x => x.Trainer)
            .Include(x => x.Visits)
            .Where(x => x.TrainerId == trainerId)
            .OrderBy(x => x.StartTime)
            .Select(x => new TrainingSessionDto
            {
                Id = x.Id,
                SectionId = x.SectionId,
                SectionName = x.Section == null ? string.Empty : x.Section.Name,
                TrainerId = x.TrainerId,
                TrainerFullName = x.Trainer == null
                    ? string.Empty
                    : x.Trainer.FirstName + " " + x.Trainer.LastName,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                MaxParticipants = x.MaxParticipants,
                VisitsCount = x.Visits.Count
            })
            .ToListAsync(cancellationToken);

        return Result<List<TrainingSessionDto>>.Success(trainingSessions);
    }

    public async Task<Result<TrainingSessionDto>> CreateAsync(
        TrainingSessionCreationRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.EndTime <= request.StartTime)
        {
            return Result<TrainingSessionDto>.Failure(TrainingSessionErrors.InvalidDateRange);
        }

        if (request.MaxParticipants <= 0)
        {
            return Result<TrainingSessionDto>.Failure(TrainingSessionErrors.InvalidMaxParticipants);
        }

        var section = await _dbContext.Sections
            .FirstOrDefaultAsync(x => x.Id == request.SectionId, cancellationToken);

        if (section is null)
        {
            return Result<TrainingSessionDto>.Failure(TrainingSessionErrors.SectionNotFound);
        }

        if (!section.IsActive)
        {
            return Result<TrainingSessionDto>.Failure(TrainingSessionErrors.SectionInactive);
        }

        var trainer = await _dbContext.Trainers
            .FirstOrDefaultAsync(x => x.Id == request.TrainerId, cancellationToken);

        if (trainer is null)
        {
            return Result<TrainingSessionDto>.Failure(TrainingSessionErrors.TrainerNotFound);
        }

        if (!trainer.IsActive)
        {
            return Result<TrainingSessionDto>.Failure(TrainingSessionErrors.TrainerInactive);
        }

        if (section.TrainerId != request.TrainerId)
        {
            return Result<TrainingSessionDto>.Failure(TrainingSessionErrors.TrainerDoesNotMatchSection);
        }

        if (request.MaxParticipants > section.Capacity)
        {
            return Result<TrainingSessionDto>.Failure(TrainingSessionErrors.CapacityExceeded);
        }

        var trainingSession = new TrainingSession
        {
            SectionId = request.SectionId,
            TrainerId = request.TrainerId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            MaxParticipants = request.MaxParticipants
        };

        _dbContext.TrainingSessions.Add(trainingSession);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<TrainingSessionDto>.Success(new TrainingSessionDto
        {
            Id = trainingSession.Id,
            SectionId = trainingSession.SectionId,
            SectionName = section.Name,
            TrainerId = trainingSession.TrainerId,
            TrainerFullName = trainer.FirstName + " " + trainer.LastName,
            StartTime = trainingSession.StartTime,
            EndTime = trainingSession.EndTime,
            MaxParticipants = trainingSession.MaxParticipants,
            VisitsCount = 0
        });
    }

    public async Task<Result> UpdateAsync(
        Guid id,
        TrainingSessionUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.EndTime <= request.StartTime)
        {
            return Result.Failure(TrainingSessionErrors.InvalidDateRange);
        }

        if (request.MaxParticipants <= 0)
        {
            return Result.Failure(TrainingSessionErrors.InvalidMaxParticipants);
        }

        var trainingSession = await _dbContext.TrainingSessions
            .Include(x => x.Visits)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (trainingSession is null)
        {
            return Result.Failure(TrainingSessionErrors.NotFound);
        }

        var section = await _dbContext.Sections
            .FirstOrDefaultAsync(x => x.Id == request.SectionId, cancellationToken);

        if (section is null)
        {
            return Result.Failure(TrainingSessionErrors.SectionNotFound);
        }

        if (!section.IsActive)
        {
            return Result.Failure(TrainingSessionErrors.SectionInactive);
        }

        var trainer = await _dbContext.Trainers
            .FirstOrDefaultAsync(x => x.Id == request.TrainerId, cancellationToken);

        if (trainer is null)
        {
            return Result.Failure(TrainingSessionErrors.TrainerNotFound);
        }

        if (!trainer.IsActive)
        {
            return Result.Failure(TrainingSessionErrors.TrainerInactive);
        }

        if (section.TrainerId != request.TrainerId)
        {
            return Result.Failure(TrainingSessionErrors.TrainerDoesNotMatchSection);
        }

        if (request.MaxParticipants > section.Capacity)
        {
            return Result.Failure(TrainingSessionErrors.CapacityExceeded);
        }

        if (request.MaxParticipants < trainingSession.Visits.Count)
        {
            return Result.Failure(TrainingSessionErrors.MaxParticipantsLessThanVisitsCount);
        }

        trainingSession.SectionId = request.SectionId;
        trainingSession.TrainerId = request.TrainerId;
        trainingSession.StartTime = request.StartTime;
        trainingSession.EndTime = request.EndTime;
        trainingSession.MaxParticipants = request.MaxParticipants;
        trainingSession.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var trainingSession = await _dbContext.TrainingSessions
            .Include(x => x.Visits)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (trainingSession is null)
        {
            return Result.Failure(TrainingSessionErrors.NotFound);
        }

        if (trainingSession.Visits.Count > 0)
        {
            return Result.Failure(TrainingSessionErrors.AlreadyHasVisits);
        }

        _dbContext.TrainingSessions.Remove(trainingSession);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}