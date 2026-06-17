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

    public async Task<TrainingSessionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TrainingSessions
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
    }

    public async Task<List<TrainingSessionDto>> GetBySectionIdAsync(
        Guid sectionId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.TrainingSessions
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
    }

    public async Task<List<TrainingSessionDto>> GetByTrainerIdAsync(
        Guid trainerId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.TrainingSessions
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
    }

    public async Task<TrainingSessionDto?> CreateAsync(
        TrainingSessionCreationRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.EndTime <= request.StartTime)
        {
            return null;
        }

        if (request.MaxParticipants <= 0)
        {
            return null;
        }

        var section = await _dbContext.Sections
            .Include(x => x.Trainer)
            .FirstOrDefaultAsync(x => x.Id == request.SectionId, cancellationToken);

        if (section is null || !section.IsActive)
        {
            return null;
        }

        if (section.TrainerId != request.TrainerId)
        {
            return null;
        }

        if (request.MaxParticipants > section.Capacity)
        {
            return null;
        }

        var trainer = await _dbContext.Trainers
            .FirstOrDefaultAsync(x => x.Id == request.TrainerId, cancellationToken);

        if (trainer is null || !trainer.IsActive)
        {
            return null;
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

        return new TrainingSessionDto
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
        };
    }

    public async Task<bool> UpdateAsync(
        Guid id,
        TrainingSessionUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.EndTime <= request.StartTime)
        {
            return false;
        }

        if (request.MaxParticipants <= 0)
        {
            return false;
        }

        var trainingSession = await _dbContext.TrainingSessions
            .Include(x => x.Visits)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (trainingSession is null)
        {
            return false;
        }

        var section = await _dbContext.Sections
            .FirstOrDefaultAsync(x => x.Id == request.SectionId, cancellationToken);

        if (section is null || !section.IsActive)
        {
            return false;
        }

        if (section.TrainerId != request.TrainerId)
        {
            return false;
        }

        if (request.MaxParticipants > section.Capacity)
        {
            return false;
        }

        if (request.MaxParticipants < trainingSession.Visits.Count)
        {
            return false;
        }

        var trainerExists = await _dbContext.Trainers
            .AnyAsync(x => x.Id == request.TrainerId && x.IsActive, cancellationToken);

        if (!trainerExists)
        {
            return false;
        }

        trainingSession.SectionId = request.SectionId;
        trainingSession.TrainerId = request.TrainerId;
        trainingSession.StartTime = request.StartTime;
        trainingSession.EndTime = request.EndTime;
        trainingSession.MaxParticipants = request.MaxParticipants;
        trainingSession.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var trainingSession = await _dbContext.TrainingSessions
            .Include(x => x.Visits)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (trainingSession is null)
        {
            return false;
        }

        if (trainingSession.Visits.Count > 0)
        {
            return false;
        }

        _dbContext.TrainingSessions.Remove(trainingSession);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}