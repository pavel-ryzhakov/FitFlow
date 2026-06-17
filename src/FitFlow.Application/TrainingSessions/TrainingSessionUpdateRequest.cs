namespace FitFlow.Application.TrainingSessions;

public class TrainingSessionUpdateRequest
{
    public Guid SectionId { get; set; }

    public Guid TrainerId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int MaxParticipants { get; set; }
}