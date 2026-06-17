using FitFlow.Domain.Enums;

namespace FitFlow.Application.Memberships;

public class MembershipUpdateRequest
{
    public MembershipType Type { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int VisitsLimit { get; set; }

    public int VisitsUsed { get; set; }

    public decimal Price { get; set; }

    public MembershipStatus Status { get; set; }
}