using FitFlow.Application.Common.Errors;

namespace FitFlow.Application.Visits;

public static class VisitErrors
{
    public static readonly Error NotFound = new(
        "Visits.NotFound",
        "Visit was not found.");

    public static readonly Error ClientNotFound = new(
        "Visits.ClientNotFound",
        "Client was not found.");

    public static readonly Error ClientInactive = new(
        "Visits.ClientInactive",
        "Client is inactive.");

    public static readonly Error MembershipNotFound = new(
        "Visits.MembershipNotFound",
        "Membership was not found.");

    public static readonly Error MembershipInactive = new(
        "Visits.MembershipInactive",
        "Membership is not active.");

    public static readonly Error MembershipNotStarted = new(
        "Visits.MembershipNotStarted",
        "Membership has not started yet.");

    public static readonly Error MembershipExpired = new(
        "Visits.MembershipExpired",
        "Membership has expired.");

    public static readonly Error VisitLimitExceeded = new(
        "Visits.VisitLimitExceeded",
        "Membership visit limit has been exceeded.");

    public static readonly Error TrainingSessionNotFound = new(
        "Visits.TrainingSessionNotFound",
        "Training session was not found.");

    public static readonly Error TrainingSessionFull = new(
        "Visits.TrainingSessionFull",
        "Training session is full.");
}