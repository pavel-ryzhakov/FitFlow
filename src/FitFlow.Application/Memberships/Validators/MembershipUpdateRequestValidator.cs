using FluentValidation;

namespace FitFlow.Application.Memberships.Validators;

public class MembershipUpdateRequestValidator : AbstractValidator<MembershipUpdateRequest>
{
    public MembershipUpdateRequestValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum();

        RuleFor(x => x.StartDate)
            .NotEmpty();

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate);

        RuleFor(x => x.VisitsLimit)
            .GreaterThan(0);

        RuleFor(x => x.VisitsUsed)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(x => x.VisitsLimit);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}