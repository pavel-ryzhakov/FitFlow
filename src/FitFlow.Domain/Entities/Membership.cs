using FitFlow.Domain.Enums;

namespace FitFlow.Domain.Entities;

public class Membership : BaseEntity
{
    public Guid ClientId { get; set; }

    public Client? Client { get; set; }

    public MembershipType Type { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int VisitsLimit { get; set; }

    public int VisitsUsed { get; set; }

    public decimal Price { get; set; }

    public MembershipStatus Status { get; set; } = MembershipStatus.Active;

    public List<Visit> Visits { get; set; } = new();

    public List<Payment> Payments { get; set; } = new();
}