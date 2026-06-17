using FitFlow.Api.Extensions;
using FitFlow.Application.Auth;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FitFlow.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<UserRegistrationRequest> _userRegistrationValidator;
    private readonly IValidator<UserLoginRequest> _userLoginValidator;

    public AuthController(
        IAuthService authService,
        IValidator<UserRegistrationRequest> userRegistrationValidator,
        IValidator<UserLoginRequest> userLoginValidator)
    {
        _authService = authService;
        _userRegistrationValidator = userRegistrationValidator;
        _userLoginValidator = userLoginValidator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(
        UserRegistrationRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _userRegistrationValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToErrorDictionary()));
        }

        var result = await _authService.RegisterAsync(request, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(
        UserLoginRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _userLoginValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToErrorDictionary()));
        }

        var result = await _authService.LoginAsync(request, cancellationToken);

        if (result.IsFailure)
        {
            return Unauthorized(result.Error);
        }

        return Ok(result.Value);
    }
}