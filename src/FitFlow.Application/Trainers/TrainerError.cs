using FitFlow.Application.Common.Errors;

namespace FitFlow.Application.Trainers;

public static class TrainerErrors
{
    public static readonly Error NotFound =
        new("Trainers.NotFound", "Trainer was not found.");

    public static readonly Error Inactive =
        new("Trainers.Inactive", "Trainer is inactive.");
}