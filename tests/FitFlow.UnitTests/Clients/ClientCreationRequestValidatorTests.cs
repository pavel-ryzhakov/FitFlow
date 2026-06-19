using FitFlow.Application.Clients;
using FitFlow.Application.Clients.Validators;
using FluentValidation.TestHelper;

namespace FitFlow.UnitTests.Clients;

public class ClientCreationRequestValidatorTests
{
    private readonly ClientCreationRequestValidator _validator = new();

    [Fact]
    public void Validate_ShouldHaveError_WhenFirstNameIsEmpty()
    {
        var request = new ClientCreationRequest
        {
            FirstName = "",
            LastName = "Petrov",
            Phone = "+79990000000",
            Email = "client@test.com",
            BirthDate = new DateTime(1994, 5, 10)
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenLastNameIsEmpty()
    {
        var request = new ClientCreationRequest
        {
            FirstName = "Ivan",
            LastName = "",
            Phone = "+79990000000",
            Email = "client@test.com",
            BirthDate = new DateTime(1994, 5, 10)
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenPhoneIsEmpty()
    {
        var request = new ClientCreationRequest
        {
            FirstName = "Ivan",
            LastName = "Petrov",
            Phone = "",
            Email = "client@test.com",
            BirthDate = new DateTime(1994, 5, 10)
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenEmailIsInvalid()
    {
        var request = new ClientCreationRequest
        {
            FirstName = "Ivan",
            LastName = "Petrov",
            Phone = "+79990000000",
            Email = "wrong-email",
            BirthDate = new DateTime(1994, 5, 10)
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenBirthDateIsInFuture()
    {
        var request = new ClientCreationRequest
        {
            FirstName = "Ivan",
            LastName = "Petrov",
            Phone = "+79990000000",
            Email = "client@test.com",
            BirthDate = DateTime.UtcNow.AddDays(1)
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.BirthDate);
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenRequestIsValid()
    {
        var request = new ClientCreationRequest
        {
            FirstName = "Ivan",
            LastName = "Petrov",
            Phone = "+79990000000",
            Email = "client@test.com",
            BirthDate = new DateTime(1994, 5, 10)
        };

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }
}