using FitFlow.Application.Common.Errors;

namespace FitFlow.Application.Sections;

public static class SectionErrors
{
    public static readonly Error NotFound = new(
        "Sections.NotFound",
        "Section was not found.");

    public static readonly Error TrainerNotFound = new(
        "Sections.TrainerNotFound",
        "Trainer was not found.");

    public static readonly Error TrainerInactive = new(
        "Sections.TrainerInactive",
        "Trainer is inactive.");
}