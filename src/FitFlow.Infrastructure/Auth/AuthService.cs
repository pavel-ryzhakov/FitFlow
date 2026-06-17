using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FitFlow.Application.Auth;
using FitFlow.Application.Common.Results;
using FitFlow.Domain.Entities;
using FitFlow.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FitFlow.Infrastructure.Auth;

public class AuthService : IAuthService
{
    private readonly FitFlowDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly JwtOptions _jwtOptions;

    public AuthService(
        FitFlowDbContext dbContext,
        IPasswordHasher<User> passwordHasher,
        IOptions<JwtOptions> jwtOptions)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<Result<AuthResponse>> RegisterAsync(
        UserRegistrationRequest request,
        CancellationToken cancellationToken = default)
    {
        var userExists = await _dbContext.Users
            .AnyAsync(x =>
                x.UserName == request.UserName ||
                x.Email == request.Email,
                cancellationToken);

        if (userExists)
        {
            return Result<AuthResponse>.Failure(AuthErrors.UserAlreadyExists);
        }

        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email,
            Role = request.Role,
            IsActive = true
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<AuthResponse>.Success(CreateAuthResponse(user));
    }

    public async Task<Result<AuthResponse>> LoginAsync(
        UserLoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x =>
                x.UserName == request.UserNameOrEmail ||
                x.Email == request.UserNameOrEmail,
                cancellationToken);

        if (user is null)
        {
            return Result<AuthResponse>.Failure(AuthErrors.InvalidCredentials);
        }

        if (!user.IsActive)
        {
            return Result<AuthResponse>.Failure(AuthErrors.UserInactive);
        }

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            return Result<AuthResponse>.Failure(AuthErrors.InvalidCredentials);
        }

        return Result<AuthResponse>.Success(CreateAuthResponse(user));
    }

    private AuthResponse CreateAuthResponse(User user)
    {
        return new AuthResponse
        {
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Role = user.Role.ToString(),
            AccessToken = CreateAccessToken(user)
        };
    }

    private string CreateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

        var signingCredentials = new SigningCredentials(
            signingKey,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}