using FitFlow.Application.Common.Results;

namespace FitFlow.Application.Payments;

public interface IPaymentService
{
    Task<List<PaymentDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Result<PaymentDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<List<PaymentDto>>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);

    Task<Result<List<PaymentDto>>> GetByMembershipIdAsync(Guid membershipId, CancellationToken cancellationToken = default);

    Task<Result<PaymentDto>> CreateAsync(PaymentCreationRequest request, CancellationToken cancellationToken = default);

    Task<Result> RefundAsync(Guid id, CancellationToken cancellationToken = default);
}