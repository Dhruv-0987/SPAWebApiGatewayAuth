using System.Net;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using ResultsPatternMinimalApis.FluentModels;

namespace ResultsPatternMinimalApis;

public static class FluentResultExtensions
{
    public static IResult ToMinimalApiResult<T>(this Result<T> result)
    {
        return result.IsSuccess ? ToMinimalApiSuccessCode(result) : ToMinimalApiErrorCode(result.ToResult());
    }

    private static IResult ToMinimalApiErrorCode(Result toResult)
    {
        return toResult switch
        {
            _ when toResult.HasError<RecordNotFound>() => TypedResults.NotFound(
                toResult.AsProblemDetails(HttpStatusCode.NotFound)),
            _ when toResult.HasError<RecordAlreadyExists>() => TypedResults.Conflict(
                toResult.AsProblemDetails(HttpStatusCode.Conflict)),
            _ => TypedResults.StatusCode(StatusCodes.Status500InternalServerError),
        };
    }

    private static IResult ToMinimalApiSuccessCode<T>(Result<T> result)
    {
        return result switch
        {
            _ when result.HasSuccess<RecordCreated>() => TypedResults.Created(),
            _ when result.HasSuccess<RecordUpdated>() => TypedResults.NoContent(),
            _ => TypedResults.Ok(result.Value)
        };
    }

    public static ProblemDetails AsProblemDetails<T>(this Result<T> result, HttpStatusCode statusCode)
    {
        var problemDetails = new ProblemDetails()
        {
            Title = result.Errors[0].Message,
            Status = (int) statusCode
        };
        problemDetails.Extensions.Add("Errors", result.Errors);
        return problemDetails;
    }

    private static ProblemDetails AsProblemDetails(this Result result, HttpStatusCode statusCode)
    {
        var problemDetails = new ProblemDetails()
        {
            Title = result.Errors[0].Message,
            Status = (int) statusCode
        };
        problemDetails.Extensions.Add("Errors", result.Errors);
        return problemDetails;
    }
}