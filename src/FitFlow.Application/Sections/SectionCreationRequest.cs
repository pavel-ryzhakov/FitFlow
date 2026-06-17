namespace FitFlow.Application.Sections;

public class SectionCreationRequest
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid TrainerId { get; set; }

    public int Capacity { get; set; }
}