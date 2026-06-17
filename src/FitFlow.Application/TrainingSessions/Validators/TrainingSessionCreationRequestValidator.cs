using FluentValidation;

namespace FitFlow.Application.TrainingSessions.Validators;

public class TrainingSessionCreationRequestValidator : AbstractValidator<TrainingSessionCreationRequest>
{
    public TrainingSessionCreationRequestValidator()
    {
        RuleFor(x => x.SectionId)
            .NotEmpty();

        RuleFor(x => x.TrainerId)
            .NotEmpty();

        RuleFor(x => x.StartTime)
            .NotEmpty();

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime);

        RuleFor(x => x.MaxParticipants)
            .GreaterThan(0);
    }
}