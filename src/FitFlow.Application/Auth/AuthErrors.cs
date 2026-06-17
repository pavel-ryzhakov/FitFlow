using FitFlow.Application.Common.Errors;

namespace FitFlow.Application.Auth;

public static class AuthErrors
{
    public static readonly Error UserAlreadyExists = new(
        "Auth.UserAlreadyExists",
        "User with this username or email already exists.");

    public static readonly Error InvalidCredentials = new(
        "Auth.InvalidCredentials",
        "Invalid username, email or password.");

    public static readonly Error UserInactive = new(
        "Auth.UserInactive",
        "User is inactive.");
}