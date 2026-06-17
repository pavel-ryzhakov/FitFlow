using FluentValidation;

namespace FitFlow.Application.Auth.Validators;

public class UserLoginRequestValidator : AbstractValidator<UserLoginRequest>
{
    public UserLoginRequestValidator()
    {
        RuleFor(x => x.UserNameOrEmail)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MaximumLength(100);
    }
}