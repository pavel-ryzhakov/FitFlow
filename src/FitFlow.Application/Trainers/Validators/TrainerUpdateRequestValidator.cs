using FluentValidation;

namespace FitFlow.Application.Trainers.Validators;

public class TrainerUpdateRequestValidator : AbstractValidator<TrainerUpdateRequest>
{
    public TrainerUpdateRequestValidator()
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

        RuleFor(x => x.Specialization)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.ExperienceYears)
            .GreaterThanOrEqualTo(0);
    }
}