using FluentValidation.Results;

namespace FitFlow.Api.Extensions;

public static class ValidationResultExtensions
{
    public static IDictionary<string, string[]> ToErrorDictionary(this ValidationResult validationResult)
    {
        return validationResult.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(x => x.ErrorMessage).ToArray());
    }
}