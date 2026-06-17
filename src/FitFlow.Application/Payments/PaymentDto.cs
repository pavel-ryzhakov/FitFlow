using FitFlow.Domain.Enums;

namespace FitFlow.Application.Payments;

public class PaymentDto
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public string ClientFullName { get; set; } = string.Empty;

    public Guid? MembershipId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaidAt { get; set; }

    public PaymentMethod Method { get; set; }

    public PaymentStatus Status { get; set; }
}