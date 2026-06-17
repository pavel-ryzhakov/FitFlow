namespace FitFlow.Domain.Entities;

public class Client : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string? Email { get; set; }

    public DateTime? BirthDate { get; set; }

    public bool IsActive { get; set; } = true;

    public List<Membership> Memberships { get; set; } = new();

    public List<Visit> Visits { get; set; } = new();

    public List<Payment> Payments { get; set; } = new();
}