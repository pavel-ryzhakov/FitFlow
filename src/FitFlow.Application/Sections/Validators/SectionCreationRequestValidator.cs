using FluentValidation;

namespace FitFlow.Application.Sections.Validators;

public class SectionCreationRequestValidator : AbstractValidator<SectionCreationRequest>
{
    public SectionCreationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.TrainerId)
            .NotEmpty();

        RuleFor(x => x.Capacity)
            .GreaterThan(0);
    }
}