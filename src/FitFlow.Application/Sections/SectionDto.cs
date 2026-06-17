namespace FitFlow.Application.Sections;

public class SectionDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid TrainerId { get; set; }

    public string TrainerFullName { get; set; } = string.Empty;

    public int Capacity { get; set; }

    public bool IsActive { get; set; }
}