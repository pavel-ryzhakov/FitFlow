using FluentValidation;

namespace FitFlow.Application.Payments.Validators;

public class PaymentCreationRequestValidator : AbstractValidator<PaymentCreationRequest>
{
    public PaymentCreationRequestValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty();

        RuleFor(x => x.MembershipId)
            .Must(id => id is null || id.Value != Guid.Empty)
            .WithMessage("Membership id must not be empty.");

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.Method)
            .IsInEnum();
    }
}