using FitFlow.Domain.Enums;

namespace FitFlow.Domain.Entities;

public class Payment : BaseEntity
{
    public Guid ClientId { get; set; }

    public Client? Client { get; set; }

    public Guid? MembershipId { get; set; }

    public Membership? Membership { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaidAt { get; set; } = DateTime.UtcNow;

    public PaymentMethod PaymentMethod { get; set; }

    public PaymentStatus Status { get; set; } = PaymentStatus.Paid;
}