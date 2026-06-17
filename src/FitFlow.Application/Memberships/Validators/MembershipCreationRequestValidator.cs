using FluentValidation;

namespace FitFlow.Application.Memberships.Validators;

public class MembershipCreationRequestValidator : AbstractValidator<MembershipCreationRequest>
{
    public MembershipCreationRequestValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty();

        RuleFor(x => x.Type)
            .IsInEnum();

        RuleFor(x => x.StartDate)
            .NotEmpty();

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate);

        RuleFor(x => x.VisitsLimit)
            .GreaterThan(0);

        RuleFor(x => x.Price)
            .GreaterThan(0);
    }
}