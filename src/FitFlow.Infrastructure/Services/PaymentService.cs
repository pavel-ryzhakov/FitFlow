using FitFlow.Application.Payments;
using FitFlow.Domain.Entities;
using FitFlow.Domain.Enums;
using FitFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitFlow.Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly FitFlowDbContext _dbContext;

    public PaymentService(FitFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<PaymentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Payments
            .AsNoTracking()
            .Include(x => x.Client)
            .OrderByDescending(x => x.PaidAt)
            .Select(x => new PaymentDto
            {
                Id = x.Id,
                ClientId = x.ClientId,
                ClientFullName = x.Client == null
                    ? string.Empty
                    : x.Client.FirstName + " " + x.Client.LastName,
                MembershipId = x.MembershipId,
                Amount = x.Amount,
                PaidAt = x.PaidAt,
                Method = x.PaymentMethod,
                Status = x.Status
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<PaymentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Payments
            .AsNoTracking()
            .Include(x => x.Client)
            .Where(x => x.Id == id)
            .Select(x => new PaymentDto
            {
                Id = x.Id,
                ClientId = x.ClientId,
                ClientFullName = x.Client == null
                    ? string.Empty
                    : x.Client.FirstName + " " + x.Client.LastName,
                MembershipId = x.MembershipId,
                Amount = x.Amount,
                PaidAt = x.PaidAt,
                Method = x.PaymentMethod,
                Status = x.Status
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<PaymentDto>> GetByClientIdAsync(
        Guid clientId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Payments
            .AsNoTracking()
            .Include(x => x.Client)
            .Where(x => x.ClientId == clientId)
            .OrderByDescending(x => x.PaidAt)
            .Select(x => new PaymentDto
            {
                Id = x.Id,
                ClientId = x.ClientId,
                ClientFullName = x.Client == null
                    ? string.Empty
                    : x.Client.FirstName + " " + x.Client.LastName,
                MembershipId = x.MembershipId,
                Amount = x.Amount,
                PaidAt = x.PaidAt,
                Method = x.PaymentMethod,
                Status = x.Status
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<PaymentDto>> GetByMembershipIdAsync(
        Guid membershipId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Payments
            .AsNoTracking()
            .Include(x => x.Client)
            .Where(x => x.MembershipId == membershipId)
            .OrderByDescending(x => x.PaidAt)
            .Select(x => new PaymentDto
            {
                Id = x.Id,
                ClientId = x.ClientId,
                ClientFullName = x.Client == null
                    ? string.Empty
                    : x.Client.FirstName + " " + x.Client.LastName,
                MembershipId = x.MembershipId,
                Amount = x.Amount,
                PaidAt = x.PaidAt,
                Method = x.PaymentMethod,
                Status = x.Status
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<PaymentDto?> CreateAsync(
        PaymentCreationRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Amount <= 0)
        {
            return null;
        }

        var client = await _dbContext.Clients
            .FirstOrDefaultAsync(x => x.Id == request.ClientId, cancellationToken);

        if (client is null || !client.IsActive)
        {
            return null;
        }

        if (request.MembershipId.HasValue)
        {
            var membershipExists = await _dbContext.Memberships
                .AnyAsync(x =>
                    x.Id == request.MembershipId.Value &&
                    x.ClientId == request.ClientId,
                    cancellationToken);

            if (!membershipExists)
            {
                return null;
            }
        }

        var payment = new Payment
        {
            ClientId = request.ClientId,
            MembershipId = request.MembershipId,
            Amount = request.Amount,
            PaidAt = DateTime.UtcNow,
            PaymentMethod = request.Method,
            Status = PaymentStatus.Paid
        };

        _dbContext.Payments.Add(payment);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new PaymentDto
        {
            Id = payment.Id,
            ClientId = payment.ClientId,
            ClientFullName = client.FirstName + " " + client.LastName,
            MembershipId = payment.MembershipId,
            Amount = payment.Amount,
            PaidAt = payment.PaidAt,
            Method = payment.PaymentMethod,
            Status = payment.Status
        };
    }

    public async Task<bool> RefundAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var payment = await _dbContext.Payments
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (payment is null)
        {
            return false;
        }

        if (payment.Status == PaymentStatus.Refunded)
        {
            return false;
        }

        payment.Status = PaymentStatus.Refunded;
        payment.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}