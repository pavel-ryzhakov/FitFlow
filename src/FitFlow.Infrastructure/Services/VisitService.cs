using FitFlow.Application;
using FitFlow.Application.Visits;
using FitFlow.Domain.Entities;
using FitFlow.Domain.Enums;
using FitFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitFlow.Infrastructure.Services;

public class VisitService : IVisitService
{
    private readonly FitFlowDbContext _dbContext;

    public VisitService(FitFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<VisitDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Visits
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.TrainingSession)
                .ThenInclude(x => x!.Section)
            .OrderByDescending(x => x.VisitedAt)
            .Select(x => new VisitDto
            {
                Id = x.Id,
                ClientId = x.ClientId,
                ClientFullName = x.Client == null
                    ? string.Empty
                    : $"{x.Client.FirstName} {x.Client.LastName}",
                MembershipId = x.MembershipId,
                TrainingSessionId = x.TrainingSessionId,
                SectionName = x.TrainingSession == null || x.TrainingSession.Section == null
                    ? null
                    : x.TrainingSession.Section.Name,
                VisitedAt = x.VisitedAt
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<VisitDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Visits
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.TrainingSession)
                .ThenInclude(x => x!.Section)
            .Where(x => x.Id == id)
            .Select(x => new VisitDto
            {
                Id = x.Id,
                ClientId = x.ClientId,
                ClientFullName = x.Client == null
                    ? string.Empty
                    : $"{x.Client.FirstName} {x.Client.LastName}",
                MembershipId = x.MembershipId,
                TrainingSessionId = x.TrainingSessionId,
                SectionName = x.TrainingSession == null || x.TrainingSession.Section == null
                    ? null
                    : x.TrainingSession.Section.Name,
                VisitedAt = x.VisitedAt
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<VisitDto>> GetByClientIdAsync(
        Guid clientId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Visits
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.TrainingSession)
                .ThenInclude(x => x!.Section)
            .Where(x => x.ClientId == clientId)
            .OrderByDescending(x => x.VisitedAt)
            .Select(x => new VisitDto
            {
                Id = x.Id,
                ClientId = x.ClientId,
                ClientFullName = x.Client == null
                    ? string.Empty
                    : $"{x.Client.FirstName} {x.Client.LastName}",
                MembershipId = x.MembershipId,
                TrainingSessionId = x.TrainingSessionId,
                SectionName = x.TrainingSession == null || x.TrainingSession.Section == null
                    ? null
                    : x.TrainingSession.Section.Name,
                VisitedAt = x.VisitedAt
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<VisitDto?> RegisterAsync(
        VisitRegistrationRequest request,
        CancellationToken cancellationToken = default)
    {
        var client = await _dbContext.Clients
            .FirstOrDefaultAsync(x => x.Id == request.ClientId, cancellationToken);

        if (client is null || !client.IsActive)
        {
            return null;
        }

        var membership = await _dbContext.Memberships
            .FirstOrDefaultAsync(x =>
                x.Id == request.MembershipId &&
                x.ClientId == request.ClientId,
                cancellationToken);

        if (membership is null)
        {
            return null;
        }

        var now = DateTime.UtcNow;

        if (membership.Status != MembershipStatus.Active)
        {
            return null;
        }

        if (membership.StartDate > now || membership.EndDate < now)
        {
            membership.Status = MembershipStatus.Expired;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return null;
        }

        if (membership.VisitsUsed >= membership.VisitsLimit)
        {
            return null;
        }

        TrainingSession? trainingSession = null;

        if (request.TrainingSessionId.HasValue)
        {
            trainingSession = await _dbContext.TrainingSessions
                .Include(x => x.Section)
                .FirstOrDefaultAsync(x => x.Id == request.TrainingSessionId.Value, cancellationToken);

            if (trainingSession is null)
            {
                return null;
            }
        }

        var visit = new Visit
        {
            ClientId = request.ClientId,
            MembershipId = request.MembershipId,
            TrainingSessionId = request.TrainingSessionId,
            VisitedAt = now
        };

        membership.VisitsUsed++;
        membership.UpdatedAt = now;

        _dbContext.Visits.Add(visit);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new VisitDto
        {
            Id = visit.Id,
            ClientId = visit.ClientId,
            ClientFullName = $"{client.FirstName} {client.LastName}",
            MembershipId = visit.MembershipId,
            TrainingSessionId = visit.TrainingSessionId,
            SectionName = trainingSession?.Section?.Name,
            VisitedAt = visit.VisitedAt
        };
    }

    public async Task<bool> CancelAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var visit = await _dbContext.Visits
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (visit is null)
        {
            return false;
        }

        var membership = await _dbContext.Memberships
            .FirstOrDefaultAsync(x => x.Id == visit.MembershipId, cancellationToken);

        if (membership is not null && membership.VisitsUsed > 0)
        {
            membership.VisitsUsed--;
            membership.UpdatedAt = DateTime.UtcNow;
        }

        _dbContext.Visits.Remove(visit);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}