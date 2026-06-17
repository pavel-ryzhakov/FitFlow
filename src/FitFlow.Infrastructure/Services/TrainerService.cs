using FitFlow.Application.Trainers;
using FitFlow.Domain.Entities;
using FitFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitFlow.Infrastructure.Services;

public class TrainerService : ITrainerService
{
    private readonly FitFlowDbContext _dbContext;

    public TrainerService(FitFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TrainerDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Trainers
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new TrainerDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Phone = x.Phone,
                Specialization = x.Specialization,
                ExperienceYears = x.ExperienceYears,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<TrainerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Trainers
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new TrainerDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Phone = x.Phone,
                Specialization = x.Specialization,
                ExperienceYears = x.ExperienceYears,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TrainerDto> CreateAsync(TrainerCreationRequest request, CancellationToken cancellationToken = default)
    {
        var trainer = new Trainer
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            Specialization = request.Specialization,
            ExperienceYears = request.ExperienceYears,
            IsActive = true
        };

        _dbContext.Trainers.Add(trainer);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new TrainerDto
        {
            Id = trainer.Id,
            FirstName = trainer.FirstName,
            LastName = trainer.LastName,
            Phone = trainer.Phone,
            Specialization = trainer.Specialization,
            ExperienceYears = trainer.ExperienceYears,
            IsActive = trainer.IsActive
        };
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateTrainerRequest request, CancellationToken cancellationToken = default)
    {
        var trainer = await _dbContext.Trainers
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (trainer is null)
        {
            return false;
        }

        trainer.FirstName = request.FirstName;
        trainer.LastName = request.LastName;
        trainer.Phone = request.Phone;
        trainer.Specialization = request.Specialization;
        trainer.ExperienceYears = request.ExperienceYears;
        trainer.IsActive = request.IsActive;
        trainer.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var trainer = await _dbContext.Trainers
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (trainer is null)
        {
            return false;
        }

        trainer.IsActive = false;
        trainer.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}