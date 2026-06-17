using FitFlow.Application.Payments;
using Microsoft.AspNetCore.Mvc;

namespace FitFlow.Api.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PaymentDto>>> GetAll(CancellationToken cancellationToken)
    {
        var payments = await _paymentService.GetAllAsync(cancellationToken);

        return Ok(payments);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PaymentDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var payment = await _paymentService.GetByIdAsync(id, cancellationToken);

        if (payment is null)
        {
            return NotFound();
        }

        return Ok(payment);
    }

    [HttpGet("client/{clientId:guid}")]
    public async Task<ActionResult<List<PaymentDto>>> GetByClientId(
        Guid clientId,
        CancellationToken cancellationToken)
    {
        var payments = await _paymentService.GetByClientIdAsync(clientId, cancellationToken);

        return Ok(payments);
    }

    [HttpGet("membership/{membershipId:guid}")]
    public async Task<ActionResult<List<PaymentDto>>> GetByMembershipId(
        Guid membershipId,
        CancellationToken cancellationToken)
    {
        var payments = await _paymentService.GetByMembershipIdAsync(membershipId, cancellationToken);

        return Ok(payments);
    }

    [HttpPost]
    public async Task<ActionResult<PaymentDto>> Create(
        PaymentCreationRequest request,
        CancellationToken cancellationToken)
    {
        var payment = await _paymentService.CreateAsync(request, cancellationToken);

        if (payment is null)
        {
            return BadRequest("Payment data is invalid.");
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = payment.Id },
            payment);
    }

    [HttpPost("{id:guid}/refund")]
    public async Task<IActionResult> Refund(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _paymentService.RefundAsync(id, cancellationToken);

        if (!result)
        {
            return BadRequest("Payment was not found or already refunded.");
        }

        return NoContent();
    }
}