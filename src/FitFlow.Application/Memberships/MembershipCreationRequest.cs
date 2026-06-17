using FitFlow.Domain.Enums;

namespace FitFlow.Application.Memberships;

public class MembershipCreationRequest
{
    public Guid ClientId { get; set; }

    public MembershipType Type { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int VisitsLimit { get; set; }

    public decimal Price { get; set; }
}