using FitFlow.Application.Trainers;
using FitFlow.Application.Trainers.Validators;
using FluentValidation.TestHelper;

namespace FitFlow.UnitTests.Trainers;

public class TrainerCreationRequestValidatorTests
{
    private readonly TrainerCreationRequestValidator _validator = new();

    [Fact]
    public void Validate_ShouldHaveError_WhenFirstNameIsEmpty()
    {
        var request = new TrainerCreationRequest
        {
            FirstName = "",
            LastName = "Petrov",
            Phone = "+79990000000",
            Specialization = "Boxing",
            ExperienceYears = 5
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenSpecializationIsEmpty()
    {
        var request = new TrainerCreationRequest
        {
            FirstName = "Ivan",
            LastName = "Petrov",
            Phone = "+79990000000",
            Specialization = "",
            ExperienceYears = 5
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Specialization);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenExperienceYearsIsNegative()
    {
        var request = new TrainerCreationRequest
        {
            FirstName = "Ivan",
            LastName = "Petrov",
            Phone = "+79990000000",
            Specialization = "Boxing",
            ExperienceYears = -1
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.ExperienceYears);
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenRequestIsValid()
    {
        var request = new TrainerCreationRequest
        {
            FirstName = "Ivan",
            LastName = "Petrov",
            Phone = "+79990000000",
            Specialization = "Boxing",
            ExperienceYears = 5
        };

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }
}