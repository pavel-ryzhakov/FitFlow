namespace FitFlow.Application.Trainers;

public class TrainerCreationRequest
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Specialization { get; set; } = string.Empty;

    public int ExperienceYears { get; set; }
}