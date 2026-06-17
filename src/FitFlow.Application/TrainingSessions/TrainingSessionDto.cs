namespace FitFlow.Application.TrainingSessions;

public class TrainingSessionDto
{
    public Guid Id { get; set; }

    public Guid SectionId { get; set; }

    public string SectionName { get; set; } = string.Empty;

    public Guid TrainerId { get; set; }

    public string TrainerFullName { get; set; } = string.Empty;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int MaxParticipants { get; set; }

    public int VisitsCount { get; set; }
}