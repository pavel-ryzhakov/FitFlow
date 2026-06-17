namespace FitFlow.Application.Auth;

public class UserLoginRequest
{
    public string UserNameOrEmail { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}