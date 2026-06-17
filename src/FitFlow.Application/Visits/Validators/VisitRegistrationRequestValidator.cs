using FluentValidation;

namespace FitFlow.Application.Visits.Validators;

public class VisitRegistrationRequestValidator : AbstractValidator<VisitRegistrationRequest>
{
    public VisitRegistrationRequestValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty();

        RuleFor(x => x.MembershipId)
            .NotEmpty();

        RuleFor(x => x.TrainingSessionId)
            .Must(id => id is null || id.Value != Guid.Empty)
            .WithMessage("Training session id must not be empty.");
    }
}