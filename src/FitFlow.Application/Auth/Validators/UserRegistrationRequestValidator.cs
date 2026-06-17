using FluentValidation;

namespace FitFlow.Application.Auth.Validators;

public class UserRegistrationRequestValidator : AbstractValidator<UserRegistrationRequest>
{
    public UserRegistrationRequestValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(150);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(100);

        RuleFor(x => x.Role)
            .IsInEnum();
    }
}