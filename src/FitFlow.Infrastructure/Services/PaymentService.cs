using FitFlow.Application.Common.Results;
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

    public async Task<Result<PaymentDto>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var payment = await _dbContext.Payments
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

        if (payment is null)
        {
            return Result<PaymentDto>.Failure(PaymentErrors.NotFound);
        }

        return Result<PaymentDto>.Success(payment);
    }

    public async Task<Result<List<PaymentDto>>> GetByClientIdAsync(
        Guid clientId,
        CancellationToken cancellationToken = default)
    {
        var clientExists = await _dbContext.Clients
            .AnyAsync(x => x.Id == clientId, cancellationToken);

        if (!clientExists)
        {
            return Result<List<PaymentDto>>.Failure(PaymentErrors.ClientNotFound);
        }

        var payments = await _dbContext.Payments
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

        return Result<List<PaymentDto>>.Success(payments);
    }

    public async Task<Result<List<PaymentDto>>> GetByMembershipIdAsync(
        Guid membershipId,
        CancellationToken cancellationToken = default)
    {
        var membershipExists = await _dbContext.Memberships
            .AnyAsync(x => x.Id == membershipId, cancellationToken);

        if (!membershipExists)
        {
            return Result<List<PaymentDto>>.Failure(PaymentErrors.MembershipNotFound);
        }

        var payments = await _dbContext.Payments
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

        return Result<List<PaymentDto>>.Success(payments);
    }

    public async Task<Result<PaymentDto>> CreateAsync(
        PaymentCreationRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Amount <= 0)
        {
            return Result<PaymentDto>.Failure(PaymentErrors.InvalidAmount);
        }

        var client = await _dbContext.Clients
            .FirstOrDefaultAsync(x => x.Id == request.ClientId, cancellationToken);

        if (client is null)
        {
            return Result<PaymentDto>.Failure(PaymentErrors.ClientNotFound);
        }

        if (!client.IsActive)
        {
            return Result<PaymentDto>.Failure(PaymentErrors.ClientInactive);
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
                return Result<PaymentDto>.Failure(PaymentErrors.MembershipNotFound);
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

        return Result<PaymentDto>.Success(new PaymentDto
        {
            Id = payment.Id,
            ClientId = payment.ClientId,
            ClientFullName = client.FirstName + " " + client.LastName,
            MembershipId = payment.MembershipId,
            Amount = payment.Amount,
            PaidAt = payment.PaidAt,
            Method = payment.PaymentMethod,
            Status = payment.Status
        });
    }

    public async Task<Result> RefundAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var payment = await _dbContext.Payments
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (payment is null)
        {
            return Result.Failure(PaymentErrors.NotFound);
        }

        if (payment.Status == PaymentStatus.Refunded)
        {
            return Result.Failure(PaymentErrors.AlreadyRefunded);
        }

        payment.Status = PaymentStatus.Refunded;
        payment.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}