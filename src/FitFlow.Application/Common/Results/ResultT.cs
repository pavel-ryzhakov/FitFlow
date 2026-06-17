using FitFlow.Application.Common.Errors;

namespace FitFlow.Application.Common.Results;

public class Result<T> : Result
{
    private Result(T value) : base(true, null)
    {
        Value = value;
    }

    private Result(Error error) : base(false, error)
    {
        Value = default;
    }

    public T? Value { get; }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value);
    }

    public static new Result<T> Failure(Error error)
    {
        return new Result<T>(error);
    }
}