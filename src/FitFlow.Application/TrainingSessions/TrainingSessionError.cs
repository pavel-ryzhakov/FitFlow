using FitFlow.Application.Common.Errors;

namespace FitFlow.Application.TrainingSessions;

public static class TrainingSessionErrors
{
    public static readonly Error NotFound = new(
        "TrainingSessions.NotFound",
        "Training session was not found.");

    public static readonly Error SectionNotFound = new(
        "TrainingSessions.SectionNotFound",
        "Section was not found.");

    public static readonly Error SectionInactive = new(
        "TrainingSessions.SectionInactive",
        "Section is inactive.");

    public static readonly Error TrainerNotFound = new(
        "TrainingSessions.TrainerNotFound",
        "Trainer was not found.");

    public static readonly Error TrainerInactive = new(
        "TrainingSessions.TrainerInactive",
        "Trainer is inactive.");

    public static readonly Error TrainerDoesNotMatchSection = new(
        "TrainingSessions.TrainerDoesNotMatchSection",
        "Trainer does not match section trainer.");

    public static readonly Error InvalidDateRange = new(
        "TrainingSessions.InvalidDateRange",
        "End time must be greater than start time.");

    public static readonly Error InvalidMaxParticipants = new(
        "TrainingSessions.InvalidMaxParticipants",
        "Max participants must be greater than zero.");

    public static readonly Error CapacityExceeded = new(
        "TrainingSessions.CapacityExceeded",
        "Max participants cannot be greater than section capacity.");

    public static readonly Error MaxParticipantsLessThanVisitsCount = new(
        "TrainingSessions.MaxParticipantsLessThanVisitsCount",
        "Max participants cannot be less than current visits count.");

    public static readonly Error AlreadyHasVisits = new(
        "TrainingSessions.AlreadyHasVisits",
        "Training session already has visits and cannot be deleted.");
}