using FitFlow.Application.Common.Results;

namespace FitFlow.Application.Auth;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(
        UserRegistrationRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<AuthResponse>> LoginAsync(
        UserLoginRequest request,
        CancellationToken cancellationToken = default);
}