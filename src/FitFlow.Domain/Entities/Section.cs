namespace FitFlow.Domain.Entities;

public class Section : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid TrainerId { get; set; }

    public Trainer? Trainer { get; set; }

    public int Capacity { get; set; }

    public bool IsActive { get; set; } = true;

    public List<TrainingSession> TrainingSessions { get; set; } = new();
}