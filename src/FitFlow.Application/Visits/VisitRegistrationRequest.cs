namespace FitFlow.Application.Visits;

public class VisitRegistrationRequest
{
    public Guid ClientId { get; set; }

    public Guid MembershipId { get; set; }

    public Guid? TrainingSessionId { get; set; }
}