using FitFlow.Api.Auth;
using FitFlow.Api.Extensions;
using FitFlow.Application.Payments;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitFlow.Api.Controllers;

[ApiController]
[Route("api/payments")]
[Authorize(Policy = AuthorizationPolicyNames.ManagementAccess)]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IValidator<PaymentCreationRequest> _paymentCreationValidator;

    public PaymentsController(
        IPaymentService paymentService,
        IValidator<PaymentCreationRequest> paymentCreationValidator)
    {
        _paymentService = paymentService;
        _paymentCreationValidator = paymentCreationValidator;
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
        var result = await _paymentService.GetByIdAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("client/{clientId:guid}")]
    public async Task<ActionResult<List<PaymentDto>>> GetByClientId(
        Guid clientId,
        CancellationToken cancellationToken)
    {
        var result = await _paymentService.GetByClientIdAsync(clientId, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("membership/{membershipId:guid}")]
    public async Task<ActionResult<List<PaymentDto>>> GetByMembershipId(
        Guid membershipId,
        CancellationToken cancellationToken)
    {
        var result = await _paymentService.GetByMembershipIdAsync(membershipId, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<PaymentDto>> Create(
        PaymentCreationRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _paymentCreationValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToErrorDictionary()));
        }

        var result = await _paymentService.CreateAsync(request, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        var payment = result.Value!;

        return CreatedAtAction(
            nameof(GetById),
            new { id = payment.Id },
            payment);
    }

    [HttpPost("{id:guid}/refund")]
    [Authorize(Policy = AuthorizationPolicyNames.AdminOnly)]
    public async Task<IActionResult> Refund(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _paymentService.RefundAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            if (result.Error == PaymentErrors.NotFound)
            {
                return NotFound(result.Error);
            }

            return BadRequest(result.Error);
        }

        return NoContent();
    }
}