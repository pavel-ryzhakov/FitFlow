using FitFlow.Domain.Enums;

namespace FitFlow.Application.Auth;

public class UserRegistrationRequest
{
    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Manager;
}