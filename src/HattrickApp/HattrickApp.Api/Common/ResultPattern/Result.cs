using FluentValidation.Results;

namespace HattrickApp.Api.Common.ResultPattern;

public class Result<T>
{
    private Result(T value)
    {
        Value = value;
        Errors = [];
        IsSuccess = true;
    }

    private Result(IReadOnlyCollection<Error> error)
    {
        Errors = error;
        Value = default;
        IsSuccess = false;
    }

    private Result(List<ValidationFailure> validationFailures)
    {
        Errors = validationFailures.ConvertAll(x => new Error(x.ErrorMessage, x.ErrorCode));
        Value = default;
        IsSuccess = false;
    }

    private IReadOnlyCollection<Error>? Errors { get; }

    private T? Value { get; }
    private bool IsSuccess { get; }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(Error error) => new([error]);
    public static Result<T> Failure(IReadOnlyCollection<Error> errors) => new(errors);
    public static Result<T> Failure(List<ValidationFailure> validationFailures) => new(validationFailures);

    public IResult HandleResult(Result<T> result)
        => IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
}
