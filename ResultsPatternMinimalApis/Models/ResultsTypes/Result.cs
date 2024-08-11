namespace ResultsPatternMinimalApis.Models.ResultsTypes;

public class Result
{
    public Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public Result Success() => new(true, Error.None);

    public Result Failure(Error error) => new(false, error);
}

public class Result<T> : Result
{
    public Result(bool isSuccess, Error error, T value) : base(isSuccess, error)
    {
        Value = value;
    }

    public T Value { get; }

    public Result<T> Success(T value) => new(true, Error.None, value);

    public Result Failure(Error error) => new(false, error);
}