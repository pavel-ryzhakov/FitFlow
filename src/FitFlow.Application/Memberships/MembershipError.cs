using FitFlow.Application.Common.Errors;

namespace FitFlow.Application.Memberships;

public static class MembershipErrors
{
    public static readonly Error NotFound = new(
        "Memberships.NotFound",
        "Membership was not found.");

    public static readonly Error ClientNotFound = new(
        "Memberships.ClientNotFound",
        "Client was not found.");

    public static readonly Error ClientInactive = new(
        "Memberships.ClientInactive",
        "Client is inactive.");

    public static readonly Error InvalidDateRange = new(
        "Memberships.InvalidDateRange",
        "End date must be greater than start date.");

    public static readonly Error InvalidVisitsLimit = new(
        "Memberships.InvalidVisitsLimit",
        "Visits limit must be greater than zero.");

    public static readonly Error InvalidVisitsUsed = new(
        "Memberships.InvalidVisitsUsed",
        "Visits used cannot be greater than visits limit.");

    public static readonly Error InvalidPrice = new(
        "Memberships.InvalidPrice",
        "Price must be greater than zero.");
}