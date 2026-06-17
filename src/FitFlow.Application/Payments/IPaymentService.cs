namespace FitFlow.Application.Payments;

public interface IPaymentService
{
    Task<List<PaymentDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<PaymentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<PaymentDto>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);

    Task<List<PaymentDto>> GetByMembershipIdAsync(Guid membershipId, CancellationToken cancellationToken = default);

    Task<PaymentDto?> CreateAsync(PaymentCreationRequest request, CancellationToken cancellationToken = default);

    Task<bool> RefundAsync(Guid id, CancellationToken cancellationToken = default);
}