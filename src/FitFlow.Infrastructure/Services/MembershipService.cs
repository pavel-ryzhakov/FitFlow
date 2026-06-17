using FitFlow.Application.Memberships;
using FitFlow.Domain.Entities;
using FitFlow.Domain.Enums;
using FitFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitFlow.Infrastructure.Services;

public class MembershipService : IMembershipService
{
    private readonly FitFlowDbContext _dbContext;

    public MembershipService(FitFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<MembershipDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Memberships
            .AsNoTracking()
            .Include(x => x.Client)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new MembershipDto
            {
                Id = x.Id,
                ClientId = x.ClientId,
                ClientFullName = x.Client == null
                    ? string.Empty
                    : $"{x.Client.FirstName} {x.Client.LastName}",
                Type = x.Type,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                VisitsLimit = x.VisitsLimit,
                VisitsUsed = x.VisitsUsed,
                Price = x.Price,
                Status = x.Status
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<MembershipDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Memberships
            .AsNoTracking()
            .Include(x => x.Client)
            .Where(x => x.Id == id)
            .Select(x => new MembershipDto
            {
                Id = x.Id,
                ClientId = x.ClientId,
                ClientFullName = x.Client == null
                    ? string.Empty
                    : $"{x.Client.FirstName} {x.Client.LastName}",
                Type = x.Type,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                VisitsLimit = x.VisitsLimit,
                VisitsUsed = x.VisitsUsed,
                Price = x.Price,
                Status = x.Status
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<MembershipDto>> GetByClientIdAsync(
        Guid clientId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Memberships
            .AsNoTracking()
            .Include(x => x.Client)
            .Where(x => x.ClientId == clientId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new MembershipDto
            {
                Id = x.Id,
                ClientId = x.ClientId,
                ClientFullName = x.Client == null
                    ? string.Empty
                    : $"{x.Client.FirstName} {x.Client.LastName}",
                Type = x.Type,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                VisitsLimit = x.VisitsLimit,
                VisitsUsed = x.VisitsUsed,
                Price = x.Price,
                Status = x.Status
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<MembershipDto?> CreateAsync(
        MembershipCreationRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.EndDate <= request.StartDate)
        {
            return null;
        }

        var client = await _dbContext.Clients
            .FirstOrDefaultAsync(x => x.Id == request.ClientId, cancellationToken);

        if (client is null)
        {
            return null;
        }

        var membership = new Membership
        {
            ClientId = request.ClientId,
            Type = request.Type,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            VisitsLimit = request.VisitsLimit,
            VisitsUsed = 0,
            Price = request.Price,
            Status = MembershipStatus.Active
        };

        _dbContext.Memberships.Add(membership);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new MembershipDto
        {
            Id = membership.Id,
            ClientId = membership.ClientId,
            ClientFullName = $"{client.FirstName} {client.LastName}",
            Type = membership.Type,
            StartDate = membership.StartDate,
            EndDate = membership.EndDate,
            VisitsLimit = membership.VisitsLimit,
            VisitsUsed = membership.VisitsUsed,
            Price = membership.Price,
            Status = membership.Status
        };
    }

    public async Task<bool> UpdateAsync(
        Guid id,
        MembershipUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.EndDate <= request.StartDate)
        {
            return false;
        }

        var membership = await _dbContext.Memberships
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (membership is null)
        {
            return false;
        }

        membership.Type = request.Type;
        membership.StartDate = request.StartDate;
        membership.EndDate = request.EndDate;
        membership.VisitsLimit = request.VisitsLimit;
        membership.VisitsUsed = request.VisitsUsed;
        membership.Price = request.Price;
        membership.Status = request.Status;
        membership.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var membership = await _dbContext.Memberships
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (membership is null)
        {
            return false;
        }

        membership.Status = MembershipStatus.Cancelled;
        membership.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}