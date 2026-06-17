namespace FitFlow.Application.Clients;

public class ClientUpdateRequest
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string? Email { get; set; }

    public DateTime? BirthDate { get; set; }

    public bool IsActive { get; set; } = true;
}