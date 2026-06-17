using FitFlow.Domain.Enums;

namespace FitFlow.Application.Payments;

public class PaymentCreationRequest
{
    public Guid ClientId { get; set; }

    public Guid? MembershipId { get; set; }

    public decimal Amount { get; set; }

    public PaymentMethod Method { get; set; }
}