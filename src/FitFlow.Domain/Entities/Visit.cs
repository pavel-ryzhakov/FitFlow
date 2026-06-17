namespace FitFlow.Domain.Entities;

public class Visit : BaseEntity
{
    public Guid ClientId { get; set; }

    public Client? Client { get; set; }

    public Guid MembershipId { get; set; }

    public Membership? Membership { get; set; }

    public Guid? TrainingSessionId { get; set; }

    public TrainingSession? TrainingSession { get; set; }

    public DateTime VisitedAt { get; set; } = DateTime.UtcNow;
}