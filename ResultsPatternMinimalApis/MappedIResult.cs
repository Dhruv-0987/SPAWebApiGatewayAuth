using ResultsPatternMinimalApis.Models.ResultsTypes;

namespace ResultsPatternMinimalApis;

public class MappedIResult<T>: IResult where T : Result<T>
{
    private readonly T _result;

    public MappedIResult(T result)
    {
        _result = result;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        IResult actionResult = _result.Match(
            onSuccess: () => Results.Ok(_result.Value),
            onFailure: error => Results.NotFound(error.Description)
        );

        return actionResult.ExecuteAsync(httpContext);
    }
}