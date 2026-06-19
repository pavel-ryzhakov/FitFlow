using System.Runtime.InteropServices.JavaScript;
using FitFlow.Application.Common.Errors;
using FitFlow.Application.Common.Results;

namespace FitFlow.UnitTests.Common;

public class ResultGenericTests
{
    [Fact]
    public void Success_ShouldCreateSuccessfulResultWithValue()
    {
        var value = "test value";

        var result = Result<string>.Success(value);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Null(result.Error);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void Failure_ShouldCreateFailedResultWithoutValue()
    {
        var error = new JSType.Error("Test.Error", "Test error message.");

        var result = Result<string>.Failure(error);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
        Assert.Null(result.Value);
    }
}