namespace FitFlow.Domain.Entities;

public class Trainer : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Specialization { get; set; } = string.Empty;

    public int ExperienceYears { get; set; }

    public bool IsActive { get; set; } = true;

    public List<Section> Sections { get; set; } = new();

    public List<TrainingSession> TrainingSessions { get; set; } = new();
}