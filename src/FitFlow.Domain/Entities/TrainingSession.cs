namespace FitFlow.Domain.Entities;

public class TrainingSession : BaseEntity
{
    public Guid SectionId { get; set; }

    public Section? Section { get; set; }

    public Guid TrainerId { get; set; }

    public Trainer? Trainer { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int MaxParticipants { get; set; }

    public List<Visit> Visits { get; set; } = new();
}