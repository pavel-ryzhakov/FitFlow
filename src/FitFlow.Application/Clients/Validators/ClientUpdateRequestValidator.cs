using FluentValidation;

namespace FitFlow.Application.Clients.Validators;

public class ClientUpdateRequestValidator : AbstractValidator<ClientUpdateRequest>
{
    public ClientUpdateRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.Email)
            .MaximumLength(150)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.UtcNow)
            .When(x => x.BirthDate.HasValue);
    }
}