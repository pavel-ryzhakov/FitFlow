namespace FitFlow.Application.Visits;

public class VisitDto
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public string ClientFullName { get; set; } = string.Empty;

    public Guid MembershipId { get; set; }

    public Guid? TrainingSessionId { get; set; }

    public string? SectionName { get; set; }

    public DateTime VisitedAt { get; set; }
}