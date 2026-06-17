using FitFlow.Application.Common.Errors;

namespace FitFlow.Application.Payments;

public static class PaymentErrors
{
    public static readonly Error NotFound = new(
        "Payments.NotFound",
        "Payment was not found.");

    public static readonly Error ClientNotFound = new(
        "Payments.ClientNotFound",
        "Client was not found.");

    public static readonly Error ClientInactive = new(
        "Payments.ClientInactive",
        "Client is inactive.");

    public static readonly Error MembershipNotFound = new(
        "Payments.MembershipNotFound",
        "Membership was not found.");

    public static readonly Error InvalidAmount = new(
        "Payments.InvalidAmount",
        "Amount must be greater than zero.");

    public static readonly Error AlreadyRefunded = new(
        "Payments.AlreadyRefunded",
        "Payment has already been refunded.");
}