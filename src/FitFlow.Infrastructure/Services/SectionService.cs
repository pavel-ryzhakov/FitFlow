using FitFlow.Application.Sections;
using FitFlow.Domain.Entities;
using FitFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace FitFlow.Infrastructure.Services;

public class SectionService : ISectionService
{
    private readonly FitFlowDbContext _dbContext;

    public SectionService(FitFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<SectionDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Sections
            .AsNoTracking()
            .Include(x => x.Trainer)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new SectionDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                TrainerId = x.TrainerId,
                TrainerFullName = x.Trainer == null
                    ? string.Empty
                    : $"{x.Trainer.FirstName} {x.Trainer.LastName}",
                Capacity = x.Capacity,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<SectionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Sections
            .AsNoTracking()
            .Include(x => x.Trainer)
            .Where(x => x.Id == id)
            .Select(x => new SectionDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                TrainerId = x.TrainerId,
                TrainerFullName = x.Trainer == null
                    ? string.Empty
                    : $"{x.Trainer.FirstName} {x.Trainer.LastName}",
                Capacity = x.Capacity,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<SectionDto?> CreateAsync(
        SectionCreationRequest request,
        CancellationToken cancellationToken = default)
    {
        var trainer = await _dbContext.Trainers
            .FirstOrDefaultAsync(x => x.Id == request.TrainerId, cancellationToken);

        if (trainer is null)
        {
            return null;
        }

        var section = new Section
        {
            Name = request.Name,
            Description = request.Description,
            TrainerId = request.TrainerId,
            Capacity = request.Capacity,
            IsActive = true
        };

        _dbContext.Sections.Add(section);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new SectionDto
        {
            Id = section.Id,
            Name = section.Name,
            Description = section.Description,
            TrainerId = section.TrainerId,
            TrainerFullName = $"{trainer.FirstName} {trainer.LastName}",
            Capacity = section.Capacity,
            IsActive = section.IsActive
        };
    }

    public async Task<bool> UpdateAsync(
        Guid id,
        SectionUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        var section = await _dbContext.Sections
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (section is null)
        {
            return false;
        }

        var trainerExists = await _dbContext.Trainers
            .AnyAsync(x => x.Id == request.TrainerId, cancellationToken);

        if (!trainerExists)
        {
            return false;
        }

        section.Name = request.Name;
        section.Description = request.Description;
        section.TrainerId = request.TrainerId;
        section.Capacity = request.Capacity;
        section.IsActive = request.IsActive;
        section.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var section = await _dbContext.Sections
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (section is null)
        {
            return false;
        }

        section.IsActive = false;
        section.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}